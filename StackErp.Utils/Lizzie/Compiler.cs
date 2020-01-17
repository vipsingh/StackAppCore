/*
 * Copyright (c) 2018 Thomas Hansen - thomas@gaiasoul.com
 *
 * Licensed under the terms of the MIT license, see the enclosed LICENSE
 * file for details.
 */

using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using lizzie.exceptions;

namespace lizzie
{
    /// <summary>
    /// Class responsible for compiling Lizzie code, and create a Lambda object
    /// out of it, which you can evaluate from your CLR code.
    /// </summary>
    public static class Compiler
    {
        /// <summary>
        /// Compile the Lizzie code found in the specified stream.
        /// </summary>
        /// <returns>The compiled lambda object.</returns>
        /// <param name="tokenizer">The tokenizer to use.</param>
        /// <param name="stream">Stream containing Lizzie code. Notice, this method does not claim ownership over
        /// your stream, and you are responsible for correctly disposing it yourself</param>
        /// <typeparam name="TContext">The type of your context object.</typeparam>
        public static Lambda<TContext> Compile<TContext>(Tokenizer tokenizer, Stream stream)
        {
            return Compile<TContext>(tokenizer.Tokenize(stream));
        }

        /// <summary>
        /// Compile the Lizzie code found in the specified streams.
        /// </summary>
        /// <returns>The compiled lambda object.</returns>
        /// <param name="tokenizer">The tokenizer to use.</param>
        /// <param name="streams">Streams containing Lizzie code. Notice, this method does not claim ownership over
        /// your streams, and you are responsible for correctly disposing your streams yourself</param>
        /// <typeparam name="TContext">The type of your context object.</typeparam>
        public static Lambda<TContext> Compile<TContext>(Tokenizer tokenizer, IEnumerable<Stream> streams)
        {
            return Compile<TContext>(tokenizer.Tokenize(streams));
        }

        /// <summary>
        /// Compile the specified Lizzie code.
        /// </summary>
        /// <returns>The compiled lambda object.</returns>
        /// <param name="tokenizer">The tokenizer to use.</param>
        /// <param name="snippet">Your Lizzie code.</param>
        /// <typeparam name="TContext">The type of your context object.</typeparam>
        public static Lambda<TContext> Compile<TContext>(Tokenizer tokenizer, string snippet)
        {
            return Compile<TContext>(tokenizer.Tokenize(snippet));
        }

        /// <summary>
        /// Compile the specified Lizzie code snippets.
        /// </summary>
        /// <returns>The compiled lambda object.</returns>
        /// <param name="tokenizer">The tokenizer to use.</param>
        /// <param name="snippets">Snippets containing your Lizzie code.</param>
        /// <typeparam name="TContext">The type of your context object.</typeparam>
        public static Lambda<TContext> Compile<TContext>(Tokenizer tokenizer, IEnumerable<string> snippets)
        {
            return Compile<TContext>(tokenizer.Tokenize(snippets));
        }

        /*
         * Sanity checks the name of a symbol.
         */
        internal static void SanityCheckSymbolName(string symbolName)
        {
            if (symbolName.IndexOfAny(new char[] { ' ', '\r', '\n', '\t' }) != -1)
                throw new LizzieParsingException($"'{symbolName}' is not a valid symbol name.");
        }

        /*
         * Common helper method for above methods, that does the heavy lifting,
         * and actually compiles our code down to a lambda object.
         */
        static Lambda<TContext> Compile<TContext>(IEnumerable<string> tokens)
        {
            /*
             * Compiling main content of code.
             *
             * NOTICE!
             * We use CompileStatements, without expecting '{' or '}' surrounding
             * our statements here, since this is the "root level" content of our code.
             */
            var tuples = CompileStatements<TContext>(tokens.GetEnumerator(), false);
            var functions = tuples.Item1;

            /*
             * Creating a function wrapping evaluation of all of our root level functions in body,
             * making sure we always return the result of the last function invocation to caller,
             * also making sure we have our root level stack object available during evaluation of
             * our lambda.
             */
            return new Lambda<TContext>((ctx, binder) => {

                /*
                 * Looping through each symbolic delegate, returning the 
                 * return value from the last to caller.
                 */
                object result = null;
                foreach (var ix in functions) {
                    result = ix(ctx, binder, null);
                }
                return result;
            });
        }

        /*
         * Compiles a lambda segment, which might be the root level
         * content of a Lizzie lambda object, or the stuff between '{' and '}'.
         *
         * If "forceClose" is true, it will expect a brace '}' to end the lambda segment,
         * and throw an exception if it finds EOF before it finds the closing '}'.
         */
        static Tuple<List<Function<TContext>>, bool> CompileStatements<TContext>(IEnumerator<string> en, bool forceClose = true)
        {
            // Creating a list of functions and returning these to caller.
            var content = new List<Function<TContext>>();
            var eof = !en.MoveNext();
            while (!eof && en.Current != "}") {

                // Compiling currently tokenized symbol.
                var tuple = CompileStatement<TContext>(en);

                // Adding function invocation to list of functions.
                content.Add(tuple.Item1);

                // Checking if we're done compiling body.
                eof = tuple.Item2;
                if (eof || en.Current == "}")
                    break; // Even if we're not at EOF, we might be at '}', ending the current body.
            }

            // Sanity checking tokenizer's content, before returning functions to caller.
            if (forceClose && en.Current != "}")
                throw new LizzieParsingException("Premature EOF while parsing code, missing an '}' character.");
            if (!forceClose && !eof && en.Current == "}")
                throw new LizzieParsingException("Unexpected closing brace '}' in code, did you add one too many '}' characters?");
            return new Tuple<List<Function<TContext>>, bool>(content, eof);
        }

        /*
         * Compiles a statement, which might be a constant, a lambda object,
         * or a function invocation.
         */
        static Tuple<Function<TContext>, bool> CompileStatement<TContext>(IEnumerator<string> en)
        {
            // Checking type of token, and acting accordingly.
            switch (en.Current) {
                case "{":
                    return CompileLambda<TContext>(en);
                case "@":
                    return CompileSymbolReference<TContext>(en);
                case "\"":
                case "'":
                    return CompileString<TContext>(en);
                default:
                    if (IsNumeric(en.Current))
                        return CompileNumber<TContext>(en);
                    else
                        return CompileSymbol<TContext>(en);
            }
        }

        /*
         * Compiles a lambda down to a function and returns the function to caller.
         */
        static Tuple<Function<TContext>, bool> CompileLambda<TContext>(IEnumerator<string> en)
        {
            // Compiling body, and retrieving functions.
            var tuples = CompileStatements<TContext>(en);
            var functions = tuples.Item1;

            /*
             * Creating a function that evaluates every function sequentially, and
             * returns the result of the last function evaluation to the caller.
             */
            Function<TContext> function = new Function<TContext>((ctx, binder, arguments) => {
                object result = null;
                foreach (var ix in functions) {
                    result = ix(ctx, binder, null);
                }
                return result;
            });

            /*
             * Making sure the body becomes evaluated lazy.
             * 
             * Notice!
             * A body is the only thing that is "by default" evaluated as "lazy" in
             * Lizzie, and does not require the '@' character to accomplish "lazy
             * evaluation".
             */
            var lazyFunction = new Function<TContext>((ctx2, binder2, arguments2) => {
                return function;
            });
            return new Tuple<Function<TContext>, bool>(lazyFunction, tuples.Item2 || !en.MoveNext());
        }

        /*
         * Compiles a literal reference down to a function and returns the function to caller.
         */
        static Tuple<Function<TContext>, bool> CompileSymbolReference<TContext>(IEnumerator<string> en)
        {
            // Sanity checking tokenizer's content, since an '@' must reference an actual symbol.
            if (!en.MoveNext())
                throw new LizzieParsingException("Unexpected EOF after '@'.");

            // Storing symbol's name and sanity checking its name.
            var symbolName = en.Current;

            // Sanity checking symbol name.
            SanityCheckSymbolName(symbolName);

            // Discarding "(" token and checking if we're at EOF.
            var eof = !en.MoveNext();

            // Checking if this is a function invocation.
            if (!eof && en.Current == "(") {

                /*
                 * Notice, since this is a literally referenced function invocation, we
                 * don't want to apply its arguments if the function is being passed around,
                 * but rather return the function as a function, which once evaluated, applies
                 * its arguments. Hence, this becomes a "lazy function evaluation", allowing us
                 * to pass in a function evaluation, that is not evaluated before the caller
                 * explicitly evaluates the function wrapping our "inner function".
                 */
                var tuple = ApplyArguments<TContext>(symbolName, en);
                var functor = tuple.Item1;
                return new Tuple<Function<TContext>, bool>(new Function<TContext>((ctx, binder, arguments) => {
                    return functor;
                }), tuple.Item2);

            } else {

                /*
                 * Creating a function that evaluates to the constant value of the symbol's name.
                 * When you use the '@' character with a symbol, this implies simply returning the
                 * symbol's name.
                 */
                return new Tuple<Function<TContext>, bool>(new Function<TContext>((ctx, binder, arguments) => {
                    return symbolName;
                }), eof);
            }
        }

        /*
         * Compiles a constant string down to a symbol and returns to caller.
         */
        static Tuple<Function<TContext>, bool> CompileString<TContext>(IEnumerator<string> en)
        {
            // Storing type of string literal quote.
            var quote = en.Current;

            // Sanity checking tokenizer's content.
            if (!en.MoveNext())
                throw new LizzieParsingException($"Unexpected EOF after {quote}.");

            // Retrieving actual string constant, and discarding closing quote character.
            var stringConstant = en.Current;
            en.MoveNext();

            // Returning a function that evaluates to the actual string's constant value.
            var function = new Function<TContext>((ctx, binder, arguments) => {
                return stringConstant;
            });
            return new Tuple<Function<TContext>, bool>(function, !en.MoveNext());
        }

        /*
         * Compiles a constant number down to a symbol and returns to caller.
         */
        static Tuple<Function<TContext>, bool> CompileNumber<TContext>(IEnumerator<string> en)
        {
            // Holds our actual number, which might be double or long.
            object numericConstant = null;

            // Checking if this is a floating point value.
            if (en.Current.IndexOfAny(new[] { '.', 'e', 'E' }) != -1) {

                // Notice, all floating point numbers are treated as double.
                var success = double.TryParse(en.Current, NumberStyles.Any, CultureInfo.InvariantCulture, out double dblResult);
                if (!success)
                    throw new LizzieRuntimeException($"Sorry, I tried my best to parse '{en.Current}' as a double, but I failed.");
                numericConstant = dblResult;

            } else {

                // Notice, all integer numbers are treated as long.
                var success = long.TryParse(en.Current, NumberStyles.Any, CultureInfo.InvariantCulture, out long longResult);
                if (!success)
                    throw new LizzieRuntimeException($"Sorry, I tried my best to parse '{en.Current}' as a long, but I failed.");
                numericConstant = longResult;
            }

            // Creates a function that evaluates to the actual constant number.
            var function = new Function<TContext>((ctx, binder, arguments) => {
                return numericConstant;
            });
            return new Tuple<Function<TContext>, bool>(function, !en.MoveNext());
        }

        /*
         * Compiles a symbolic reference down to a function invocation and returns
         * that function to caller.
         */
        static Tuple<Function<TContext>, bool> CompileSymbol<TContext>(IEnumerator<string> en)
        {
            // Retrieving symbol's name and sanity checking it.
            var symbolName = en.Current;
            SanityCheckSymbolName(symbolName);

            // Discarding "(" token and checking if we're at EOF.
            var eof = !en.MoveNext();

            // Checking if this is a function invocation.
            if (!eof && en.Current == "(") {
                
                if(symbolName == "ev")
                {
                    return ApplyEvalArguments<TContext>(symbolName, en);
                } else
                // Function invocation, making sure we apply arguments,
                    return ApplyArguments<TContext>(symbolName, en);

            } else {

                // Referencing value of symbol.
                return new Tuple<Function<TContext>, bool>(new Function<TContext>((ctx, binder, arguments) => {

                    return binder[symbolName];

                }), eof);
            }
        }

        /*
         * Applies arguments to a function invoction, such that they're evaluated at runtime.
         */
        static Tuple<Function<TContext>, bool> ApplyArguments<TContext>(string symbolName, IEnumerator<string> en)
        {
            // Used to hold arguments before they're being applied inside of function evaluation.
            var arguments = new List<Function<TContext>>();

            // Sanity checking tokenizer's content.
            if (!en.MoveNext())
                throw new LizzieParsingException("Unexpected EOF while parsing function invocation.");

            // Looping through all arguments, if there are any.
            while (en.Current != ")") {

                // Compiling current argument.
                var tuple = CompileStatement<TContext>(en);
                arguments.Add(tuple.Item1);
                if (en.Current == ")")
                    break; // And we are done parsing arguments.

                // Sanity checking tokenizer's content, and discarding "," token.
                if (en.Current != ",")
                    throw new LizzieParsingException($"Syntax error in arguments to '{symbolName}', expected ',' separating arguments and found '{en.Current}'.");
                if (!en.MoveNext())
                    throw new LizzieParsingException("Unexpected EOF while parsing arguments to function invocation.");
            }

            /*
             * Creates a function invocation that evaluates its arguments at runtime.
             */
            return new Tuple<Function<TContext>, bool>(new Function<TContext>((ctx, binder, args) => {

                // Applying arguments.
                var appliedArguments = new Arguments(arguments.Select(ix => ix(ctx, binder, new Arguments())));
                if (appliedArguments.Count == 1 && appliedArguments.Get(0) is Arguments explicitlyApplied) {
                    appliedArguments = explicitlyApplied;
                }

                // Retrieving symbol's value and doing some basic sanity checks.
                var symbol = binder[symbolName];
                if (symbol == null)
                    throw new LizzieRuntimeException($"Symbol '{symbolName}' is null.");
                if (symbol is Function<TContext> functor)
                    return functor(ctx, binder, appliedArguments); // Success!
                throw new LizzieRuntimeException($"'{symbolName}' is not a function, but a '{symbol.GetType().FullName}'");
            }), !en.MoveNext());
        }

        /*
         * Returns true if this is a numeric value, which might be floating point
         * value, or an integer value.
         */
        static bool IsNumeric(string symbol)
        {
            // To support scientific notation, split the number into the base and exponent parts, 
            // which will be seperated by an 'E' character.
            var parts = symbol.Split('e', 'E');

            // All valid numbers will either have one or two parts, the coefficient will always be the first part.
            string coefficient = SanitizeSign(parts[0]);

            // If we have two parts we need to check the exponent.
            if (parts.Length == 2) {

                var exponent = SanitizeSign(parts[1]);
                if (exponent.Any(ix => !char.IsDigit(ix)))
                    return false;

            } else if (parts.Length != 1) {

                return false;
            }

            // If we are this far then we just need to check that the coefficient.
            return coefficient.All(ix => char.IsDigit(ix) || ix == '.');
        }

        /*
         * Returns the string with the first character removed if the first character is 
         * a '+' or '-', as long as that is not the only character in the string.
         */
        private static string SanitizeSign(string number)
        {
            if (number.Length > 1 && (number[0] == '-' || number[0] == '+')) {
                number = number.Substring(1);
            }
            return number;
        }

        //vipin
        static Tuple<Function<TContext>, bool> ApplyEvalArguments<TContext>(string symbolName, IEnumerator<string> en)
        {
            // Used to hold arguments before they're being applied inside of function evaluation.
            var arguments = new List<Function<TContext>>();

            // Sanity checking tokenizer's content.
            if (!en.MoveNext())
                throw new LizzieParsingException("Unexpected EOF while parsing function invocation.");

            string d = "(";
            // Looping through all arguments, if there are any.
            while (en.Current != ")") {
                d += en.Current;
                // Compiling current argument.
                var tuple = CompileStatement<TContext>(en);
                arguments.Add(tuple.Item1);
                
                d += en.Current;

                if (en.Current == ")")
                    break; // And we are done parsing arguments.

                // Sanity checking tokenizer's content, and discarding "," token.
                // if (en.Current != ",")
                //     throw new LizzieParsingException($"Syntax error in arguments to '{symbolName}', expected ',' separating arguments and found '{en.Current}'.");
                if (!en.MoveNext())
                    throw new LizzieParsingException("Unexpected EOF while parsing arguments to function invocation.");
            }
            arguments.Add(new Function<TContext>((ctx, binder, args) => {
                return d;
            }));

            /*
             * Creates a function invocation that evaluates its arguments at runtime.
             */
            return new Tuple<Function<TContext>, bool>(new Function<TContext>((ctx, binder, args) => {

                // Applying arguments.
                var appliedArguments = new Arguments(arguments.Select(ix => ix(ctx, binder, new Arguments())));
                if (appliedArguments.Count == 1 && appliedArguments.Get(0) is Arguments explicitlyApplied) {
                    appliedArguments = explicitlyApplied;
                }

                // Retrieving symbol's value and doing some basic sanity checks.
                var symbol = binder[symbolName];
                if (symbol == null)
                    throw new LizzieRuntimeException($"Symbol '{symbolName}' is null.");
                if (symbol is Function<TContext> functor)
                    return functor(ctx, binder, appliedArguments); // Success!
                throw new LizzieRuntimeException($"'{symbolName}' is not a function, but a '{symbol.GetType().FullName}'");
            }), !en.MoveNext());
        }

    }
}