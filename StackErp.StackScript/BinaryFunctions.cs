using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Esprima;
using Esprima.Ast;
using Esprima.Utils;
using Newtonsoft.Json;
using System.Globalization;

namespace StackErp.StackScript
{
    internal static class BinaryFunctions
    {
        static Dictionary<BinaryOperator, Function> _binder;
        static BinaryFunctions()
        {
            _binder = new Dictionary<BinaryOperator, Function>();
            _binder.Add(BinaryOperator.Plus, Add);
            _binder.Add(BinaryOperator.Minus, Subtract);
            _binder.Add(BinaryOperator.Times, Multiply);
            _binder.Add(BinaryOperator.Divide, Devide);
            _binder.Add(BinaryOperator.Equal, IsEqual);
            _binder.Add(BinaryOperator.NotEqual, IsNotEqual);
            _binder.Add(BinaryOperator.Greater, Greater);
            _binder.Add(BinaryOperator.GreaterOrEqual, GreaterEqual);
            _binder.Add(BinaryOperator.Less, Less);
            _binder.Add(BinaryOperator.LessOrEqual, LessEqual);

            _binder.Add(BinaryOperator.LogicalAnd, LogicalAnd);
            _binder.Add(BinaryOperator.LogicalOr, LogicalOr);
        }

        internal static Function Get(BinaryOperator op)
        {
            return _binder[op];
        }

        internal static Function Add => new Function((arguments) =>
        {
            if (arguments.Count <= 1 )
                throw new ScriptException("Invalid arguments in Add");

            dynamic result = arguments.Get(0);
            foreach (dynamic ix in arguments.Skip(1)) {
                result = result + ix;
            }

            return result;
        });

        internal static Function Subtract => new Function((arguments) =>
        {
            if (arguments.Count <= 1 )
                throw new ScriptException("Invalid arguments in Subtract");

            dynamic result = arguments.Get(0);
            foreach (dynamic ix in arguments.Skip(1)) {
                result = result - ix;
            }

            return result;
        });

        internal static Function Multiply => new Function((arguments) =>
        {
            if (arguments.Count <= 1 )
                throw new ScriptException("Invalid arguments in Multiply");

            dynamic a1 = arguments.Get(0);
            dynamic a2 = arguments.Get(1);
            
            return a1 * a2;
        });

        internal static Function Devide => new Function((arguments) =>
        {
            if (arguments.Count <= 1 )
                throw new ScriptException("Invalid arguments in Devide");

            dynamic a1 = arguments.Get(0);
            dynamic a2 = arguments.Get(1);
            
            return a1 / a2;
        });

        internal static Function IsEqual => new Function((arguments) =>
        {
            if (arguments.Count <= 1 )
                throw new ScriptException("Invalid arguments in IsEqual"); 

            var arg1 = arguments.Get(0);
            var arg2 = arguments.Get(1);
            if (arg1 == null && arg2 != null || arg1 != null && arg2 == null)
                return (object)false;
            if (arg1 != null && !arg1.Equals(arg2))
                return (object)false;
                
            return (object)true;
        });

        internal static Function IsNotEqual => new Function((arguments) =>
        {
            if (arguments.Count <= 1 )
                throw new ScriptException("Invalid arguments in IsNotEqual");

            var arg1 = arguments.Get(0);
            var arg2 = arguments.Get(1);
            if (arg1 == null && arg2 != null || arg1 != null && arg2 == null)
                return (object)true;

            if (arg1 != null && !arg1.Equals(arg2))
                return (object)true;
                
            return (object)false;
        });

        internal static Function Greater => new Function((arguments) =>
        {
            if (arguments.Count <= 1 )
                throw new ScriptException("Invalid arguments in Greater"); 

            dynamic arg1 = arguments.Get(0);
            dynamic arg2 = arguments.Get(1);
            if (arg1 == null || arg2 == null)
                throw new ScriptException("Invalid arguments in Greater (Null Value)"); 

            if (arg1 > arg2)
                return (object)true;
                
            return (object)false;
        });

        internal static Function Less => new Function((arguments) =>
        {
            if (arguments.Count <= 1 )
                throw new ScriptException("Invalid arguments in Less"); 

            dynamic arg1 = arguments.Get(0);
            dynamic arg2 = arguments.Get(1);
            if (arg1 == null || arg2 == null)
                throw new ScriptException("Invalid arguments in Less (Null Value)"); 

            if (arg1 < arg2)
                return (object)true;
                
            return (object)false;
        });

        internal static Function GreaterEqual => new Function((arguments) =>
        {
            if (arguments.Count <= 1 )
                throw new ScriptException("Invalid arguments in GreaterEqual"); 

            dynamic arg1 = arguments.Get(0);
            dynamic arg2 = arguments.Get(1);
            if (arg1 == null || arg2 == null)
                throw new ScriptException("Invalid arguments in GreaterEqual (Null Value)"); 

            if (arg1 >= arg2)
                return (object)true;
                
            return (object)false;
        });

        internal static Function LessEqual => new Function((arguments) =>
        {
            if (arguments.Count <= 1 )
                throw new ScriptException("Invalid arguments in LessEqual"); 

            dynamic arg1 = arguments.Get(0);
            dynamic arg2 = arguments.Get(1);
            if (arg1 == null || arg2 == null)
                throw new ScriptException("Invalid arguments in LessEqual (Null Value)"); 

            if (arg1 <= arg2)
                return (object)true;
                
            return (object)false;
        });

        internal static Function LogicalAnd => new Function((arguments) =>
        {
            if (arguments.Count <= 1 )
                throw new ScriptException("Invalid arguments in LogicalAnd"); 

            var arg1 = arguments.Get(0);
            var arg2 = arguments.Get(1);
            if (arg1 == (object)true && arg2 == (object)true )
                return (object)true;
                
            return (object)false;
        });

        internal static Function LogicalOr => new Function((arguments) =>
        {
            if (arguments.Count <= 1 )
                throw new ScriptException("Invalid arguments in LogicalOr"); 

            var arg1 = arguments.Get(0);
            var arg2 = arguments.Get(1);

            if (arg1 == (object)true || arg2 == (object)true )
                return (object)true;
                
            return (object)false;
        });
    }
}