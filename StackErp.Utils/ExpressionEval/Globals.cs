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

public static class Globals
{
    internal static bool varEq(string v1, string v2)
    {
        int lv1, lv2;
        if (v1 == null)
            lv1 = 0;
        else
            lv1 = v1.Length;
        if (v2 == null)
            lv2 = 0;
        else
            lv2 = v2.Length;

        if (lv1 != lv2)
            return false;
        if (lv1 == 0)
            return true;

        char c1, c2;

        for (int i = 0; i <= lv1 - 1; i++)
        {
            c1 = v1[i];
            c2 = v2[i];
            switch (c1)
            {
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                    {
                        if (c2 != c1 && (int)c2 != (int)c1 - 32)
                            return false;
                        break;
                    }

                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                case 'H':
                case 'I':
                case 'J':
                case 'K':
                case 'L':
                case 'M':
                case 'N':
                case 'O':
                case 'P':
                case 'Q':
                case 'R':
                case 'S':
                case 'T':
                case 'U':
                case 'V':
                case 'W':
                case 'X':
                case 'Y':
                case 'Z':
                    {
                        if (c2 != c1 && (int)c2 != (int)c1 + 32)
                            return false;
                        break;
                    }

                case '-':
                case '_':
                case '.':
                    {
                        if (c2 != c1 && c2 != '_' && c2 != '.')
                            return false;
                        break;
                    }

                //case '_':
                //    {
                //        if (c2 != c1 && c2 != '-')
                //            return false;
                //        break;
                //    }

                default:
                    {
                        if (c2 != c1)
                            return false;
                        break;
                    }
            }
        }
        return true;
    }

    internal static EvalType GetObjectType(object o)
    {
        if (o == null)
            return EvalType.Unknown;
        else
        {
            Type t = o.GetType();
            return GetEvalType(t);
        }
    }

    internal static EvalType GetEvalType(Type t)
    {
        if (t == typeof(float) | t == typeof(double) | t == typeof(decimal) | t == typeof(Int16) | t == typeof(Int32) | t == typeof(Int64) | t == typeof(byte) | t == typeof(UInt16) | t == typeof(UInt32) | t == typeof(UInt64))
            return EvalType.Number;
        else if (t == typeof(DateTime))
            return EvalType.Date;
        else if (t == typeof(bool))
            return EvalType.Boolean;
        else if (t == typeof(string))
            return EvalType.String;
        else
            return EvalType.Object;
    }

    internal static System.Type GetSystemType(EvalType t)
    {
        switch (t)
        {
            case EvalType.Boolean:
                {
                    return typeof(bool);
                }

            case EvalType.Date:
                {
                    return typeof(DateTime);
                }

            case EvalType.Number:
                {
                    return typeof(double);
                }

            case EvalType.String:
                {
                    return typeof(string);
                }

            default:
                {
                    return typeof(object);
                }
        }
    }

    public static bool TBool(iEvalTypedValue o)
    {
        return System.Convert.ToBoolean(o.Value);
    }

    public static DateTime TDate(iEvalTypedValue o)
    {
        return (DateTime)o.Value;
    }

    public static double TNum(iEvalTypedValue o)
    {
        return System.Convert.ToDouble(o.Value);
    }

    public static string TStr(iEvalTypedValue o)
    {
        return System.Convert.ToString(o.Value);
    }
}

internal enum ePriority
{
    none = 0,
    or = 1,
    and = 2,
    not = 3,
    equality = 4,
    concat = 5,
    plusminus = 6,
    muldiv = 7,
    percent = 8,
    unaryminus = 9
}

public enum VariableComplexity
{
    normal
}

public enum EvalType
{
    Unknown,
    Number,
    Boolean,
    String,
    Date,
    Object
}

public enum eParserSyntax
{
    cSharp,
    Vb
}

public enum eTokenType
{
    none,
    end_of_formula,
    operator_plus,
    operator_minus,
    operator_mul,
    operator_div,
    operator_percent,
    open_parenthesis,
    comma,
    dot,
    close_parenthesis,
    operator_ne,
    operator_gt,
    operator_ge,
    operator_eq,
    operator_le,
    operator_lt,
    operator_and,
    operator_or,
    operator_not,
    operator_concat,
    operator_if,
    value_identifier,
    value_true,
    value_false,
    value_number,
    value_string,
    value_date,
    open_bracket,
    close_bracket
}

public class VariableNotFoundException : Exception
{
    public readonly string VariableName;

    public VariableNotFoundException(string variableName, Exception innerException = null) : base(variableName + " was not found", null)
    {
        this.VariableName = variableName;
    }
}


}
