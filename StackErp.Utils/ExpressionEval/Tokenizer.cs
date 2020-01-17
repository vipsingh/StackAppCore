   using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using Microsoft.VisualBasic;

namespace StackErp.Utils.ExpressionEval
{
 

public class tokenizer
{
    private string mString;
    private int mLen;
    private int mPos;
    private char mCurChar;
    private parser mParser;
    private eParserSyntax mSyntax;

    public int startpos;
    public eTokenType type;
    public System.Text.StringBuilder value = new System.Text.StringBuilder();

    internal tokenizer(parser Parser, string str, eParserSyntax syntax = eParserSyntax.Vb)
    {
        mString = str;
        mLen = str.Length;
        mSyntax = syntax;
        mPos = 0;
        mParser = Parser;
        NextChar();   // start the machine
    }

    internal void RaiseError(string msg, Exception ex = null)
    {
        if (ex is Evaluator.parserException)
            msg += ". " + ex.Message;
        else
        {
            msg += " " + " at position " + startpos;
            if (ex != null)
                msg += ". " + ex.Message;
        }
        throw new Evaluator.parserException(msg, this.mString, this.mPos);
    }

    internal void RaiseUnexpectedToken(string msg = null)
    {
        if (msg.Length == 0)
            msg = "";
        else
            msg += "; ";
        RaiseError(msg + "Unexpected " + type.ToString().Replace('_', ' ') + " : " + value.ToString());
    }

    internal void RaiseWrongOperator(eTokenType tt, object ValueLeft, object valueRight, string msg = null)
    {
        if (msg.Length > 0)
        {
            msg.Replace("[op]", tt.GetType().ToString());
            msg += ". ";
        }
        msg = "Cannot apply the operator " + tt.ToString();
        if (ValueLeft == null)
            msg += " on nothing";
        else
            msg += " on a " + ValueLeft.GetType().ToString();
        if (valueRight != null)
            msg += " and a " + valueRight.GetType().ToString();
        RaiseError(msg);
    }

    private bool IsOp()
    {
        return mCurChar == '+'
       | mCurChar == '-'
       | mCurChar == '–'
       | mCurChar == '%'
       | mCurChar == '/'
       | mCurChar == '('
       | mCurChar == ')'
       | mCurChar == '.';
    }

    public void NextToken()
    {
        value.Length = 0;
        type = eTokenType.none;
        do
        {
            startpos = mPos;
            switch (mCurChar)
            {
                case default(Char):
                    {
                        type = eTokenType.end_of_formula;
                        break;
                    }

                case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    {
                        ParseNumber();
                        break;
                    }

                case '-':
                case '–':
                    {
                        NextChar();
                        type = eTokenType.operator_minus;
                        break;
                    }

                case '+':
                    {
                        NextChar();
                        type = eTokenType.operator_plus;
                        break;
                    }

                case '*':
                    {
                        NextChar();
                        type = eTokenType.operator_mul;
                        break;
                    }

                case '/':
                    {
                        NextChar();
                        type = eTokenType.operator_div;
                        break;
                    }

                case '%':
                    {
                        NextChar();
                        type = eTokenType.operator_percent;
                        break;
                    }

                case '(':
                    {
                        NextChar();
                        type = eTokenType.open_parenthesis;
                        break;
                    }

                case ')':
                    {
                        NextChar();
                        type = eTokenType.close_parenthesis;
                        break;
                    }

                case '<':
                    {
                        NextChar();
                        if (mCurChar == '=')
                        {
                            NextChar();
                            type = eTokenType.operator_le;
                        }
                        else if (mCurChar == '>')
                        {
                            NextChar();
                            type = eTokenType.operator_ne;
                        }
                        else
                            type = eTokenType.operator_lt;
                        break;
                    }

                case '>':
                    {
                        NextChar();
                        if (mCurChar == '=')
                        {
                            NextChar();
                            type = eTokenType.operator_ge;
                        }
                        else
                            type = eTokenType.operator_gt;
                        break;
                    }

                case ',':
                    {
                        NextChar();
                        type = eTokenType.comma;
                        break;
                    }

                case '=':
                    {
                        NextChar();
                        type = eTokenType.operator_eq;
                        break;
                    }

                case '.':
                    {
                        NextChar();
                        type = eTokenType.dot;
                        break;
                    }

                case '\'':
                case '"':
                    {
                        ParseString(true);
                        type = eTokenType.value_string;
                        break;
                    }

                case '#':
                    {
                        ParseDate();
                        break;
                    }

                case '&':
                    {
                        NextChar();
                        type = eTokenType.operator_concat;
                        break;
                    }

                case '[':
                    {
                        NextChar();
                        type = eTokenType.open_bracket;
                        break;
                    }

                case ']':
                    {
                        NextChar();
                        type = eTokenType.close_bracket;
                        break;
                    }

                //case (char)0 <= mCurChar && mCurChar <= ' ':
                //    {
                //        break;
                //    }

                default:
                    {
                        ParseIdentifier();
                        break;
                    }
            }

            if (type != eTokenType.none)
                break;
            NextChar();
        }
        while (true);
    }

    private void NextChar()
    {
        if (mPos < mLen)
        {
            mCurChar = mString[mPos];
            if (mCurChar == (char)(147) | mCurChar == (char)(148))
                mCurChar = '"';
            if (mCurChar == (char)(145) | mCurChar == (char)(146))
                mCurChar = '\'';
            mPos += 1;
        }
        else
            mCurChar = default(Char);
    }

    private void ParseNumber()
    {
        type = eTokenType.value_number;
        while (mCurChar >= '0' & mCurChar <= '9')
        {
            value.Append(mCurChar);
            NextChar();
        }
        if (mCurChar == '.')
        {
            value.Append(mCurChar);
            NextChar();
            while (mCurChar >= '0' & mCurChar <= '9')
            {
                value.Append(mCurChar);
                NextChar();
            }
        }
    }

    private void ParseIdentifier()
    {
        while ((mCurChar >= '0' & mCurChar <= '9') | (mCurChar >= 'a' & mCurChar <= 'z') | (mCurChar >= 'A' & mCurChar <= 'Z') | (mCurChar >= 'A' & mCurChar <= 'Z') | (mCurChar >= (char)(128)) | (mCurChar == '_'))
        {
            value.Append(mCurChar);
            NextChar();
        }
        switch (value.ToString())
        {
            case "and":
                {
                    type = eTokenType.operator_and;
                    break;
                }

            case "or":
                {
                    type = eTokenType.operator_or;
                    break;
                }

            case "not":
                {
                    type = eTokenType.operator_not;
                    break;
                }

            case "true":
            case "yes":
                {
                    type = eTokenType.value_true;
                    break;
                }

            case "if":
                {
                    type = eTokenType.operator_if;
                    break;
                }

            case "false":
            case "no":
                {
                    type = eTokenType.value_false;
                    break;
                }

            default:
                {
                    type = eTokenType.value_identifier;
                    break;
                }
        }
    }

    private void ParseString(bool InQuote)
    {
        char OriginalChar =Char.MinValue;
        if (InQuote)
        {
            OriginalChar = mCurChar;
            NextChar();
        }

        char PreviousChar;
        while (mCurChar != default(Char))
        {
            if (InQuote && mCurChar == OriginalChar)
            {
                NextChar();
                if (mCurChar == OriginalChar)
                    value.Append(mCurChar);
                else
                    // End of String
                    return;
            }
            else if (mCurChar == '%')
            {
                NextChar();
                if (mCurChar == '[')
                {
                    NextChar();
                    System.Text.StringBuilder SaveValue = value;
                    int SaveStartPos = startpos;
                    this.value = new System.Text.StringBuilder();
                    this.NextToken(); // restart the tokenizer for the subExpr
                    object subExpr = null;
                    try
                    {
                        // subExpr = mParser.ParseExpr(0, ePriority.none)
                        if (subExpr == null)
                            this.value.Append("<nothing>");
                        else
                            this.value.Append(Evaluator.ConvertToString(subExpr));
                    }
                    catch (Exception ex)
                    {
                        // XML don't like < and >
                        this.value.Append("[Error " + ex.Message + "]");
                    }
                    SaveValue.Append(value.ToString());
                    value = SaveValue;
                    startpos = SaveStartPos;
                }
                else
                    value.Append('%');
            }
            else
            {
                value.Append(mCurChar);
                NextChar();
            }
        }
        if (InQuote)
            RaiseError("Incomplete string, missing " + OriginalChar + "; String started");
    }

    private void ParseDate()
    {
        NextChar(); // eat the #
        int zone = 0;
        while ((mCurChar >= '0' & mCurChar <= '9') | (mCurChar == '/') | (mCurChar == ':') | (mCurChar == ' '))
        {
            value.Append(mCurChar);
            NextChar();
        }
        if (mCurChar != '#')
            RaiseError("Invalid date should be #dd/mm/yyyy#");
        else
            NextChar();
        type = eTokenType.value_date;
    }
}

}
