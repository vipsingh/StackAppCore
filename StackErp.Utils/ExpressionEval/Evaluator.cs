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
using System.Data;
using System.Collections;

namespace StackErp.Utils.ExpressionEval
{

public class Evaluator
{
    internal ArrayList mEnvironmentFunctionsList;
    public bool RaiseVariableNotFoundException;
    public readonly eParserSyntax Syntax;
    public readonly bool CaseSensitive;

    public Evaluator(eParserSyntax syntax = eParserSyntax.Vb, bool caseSensitive = false)
    {
        this.Syntax = syntax;
        this.CaseSensitive = caseSensitive;
        mEnvironmentFunctionsList = new ArrayList();
    }

    public void AddEnvironmentFunctions(object obj)
    {
        if (obj == null)
            return;
        if (!mEnvironmentFunctionsList.Contains(obj))
            mEnvironmentFunctionsList.Add(obj);
    }

    public void RemoveEnvironmentFunctions(object obj)
    {
        if (mEnvironmentFunctionsList.Contains(obj))
            mEnvironmentFunctionsList.Remove(obj);
    }

    public opCode Parse(string str)
    {
        return new parser(this).Parse(str);
    }

    public static string ConvertToString(object value)
    {
        if (value is string)
            return (string)value;
        else if (value == null)
            return string.Empty;
        else if (value is DateTime)
        {
            DateTime d = (DateTime)value;
            if (d.TimeOfDay.TotalMilliseconds > 0)
                return d.ToString();
            else
                return d.ToShortDateString();
        }
        else if (value is decimal)
        {
            decimal d = (decimal)value;
            if ((d % 1) != 0)
                return d.ToString("#,##0.00");
            else
                return d.ToString("#,##0");
        }
        else if (value is double)
        {
            double d = (double)value;
            if ((d % 1) != 0)
                return d.ToString("#,##0.00");
            else
                return d.ToString("#,##0");
        }
        else if (value is object)
            return value.ToString();

        return value.ToString();
    }

    public class parserException : Exception
    {
        public readonly string formula;
        public readonly int pos;

        internal parserException(string str, string formula, int pos) : base(str)
        {
            this.formula = formula;
            this.pos = pos;
        }
    }
}

}
