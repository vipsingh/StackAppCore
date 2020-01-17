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

/*
TODO:: <variable> = if(<Boolean expression>, expression1, expression2)
*/

namespace StackErp.Utils.ExpressionEval
{

internal class parser
{
    private tokenizer mTokenizer;
    private Evaluator mEvaluator;

    public parser(Evaluator evaluator)
    {
        mEvaluator = evaluator;
    }

    public opCode Parse(string str)
    {
        if (str == null)
            str = string.Empty;
        mTokenizer = new tokenizer(this, str);
        mTokenizer.NextToken();
        opCode res = ParseExpr(null/* TODO Change to default(_) if this is not a reference type */, ePriority.none);
        if (mTokenizer.type == eTokenType.end_of_formula)
        {
            if (res == null)
                res = new opCodeImmediate(EvalType.String, string.Empty);
            return res;
        }
        else
            mTokenizer.RaiseUnexpectedToken();

        return res;
    }

    private opCode ParseExpr(opCode Acc, ePriority priority)
    {
        opCode ValueLeft = null, valueRight = null;

        do
        {
            switch (mTokenizer.type)
            {
                case eTokenType.operator_minus:
                    {
                        // unary minus operator
                        mTokenizer.NextToken();
                        ValueLeft = ParseExpr(null/* TODO Change to default(_) if this is not a reference type */, ePriority.unaryminus);
                        ValueLeft = new opCodeUnary(eTokenType.operator_minus, ValueLeft);
                        goto Foo;
                        break;
                        
                    }

                case eTokenType.operator_plus:
                    {
                        // unary minus operator
                        mTokenizer.NextToken();
                        break;
                    }

                case eTokenType.operator_not:
                    {
                        mTokenizer.NextToken();
                        ValueLeft = ParseExpr(null/* TODO Change to default(_) if this is not a reference type */, ePriority.not);
                        ValueLeft = new opCodeUnary(eTokenType.operator_not, ValueLeft);
                        goto Foo;
                        break;
                    }

                case eTokenType.value_identifier:
                    {
                        
                        ParseIdentifier(ref ValueLeft);
                        goto Foo;
                        break;
                    }

                case eTokenType.value_true:
                    {
                        ValueLeft = new opCodeImmediate(EvalType.Boolean, true);
                        mTokenizer.NextToken();
                        goto Foo;
                        break;
                    }

                case eTokenType.value_false:
                    {
                        ValueLeft = new opCodeImmediate(EvalType.Boolean, false);
                        mTokenizer.NextToken();
                        goto Foo;
                        break;
                    }

                case eTokenType.value_string:
                    {
                        ValueLeft = new opCodeImmediate(EvalType.String, mTokenizer.value.ToString());
                        mTokenizer.NextToken();
                        goto Foo;
                        break;
                    }

                case eTokenType.value_number:
                    {
                        try
                        {
                            ValueLeft = new opCodeImmediate(EvalType.Number, double.Parse(mTokenizer.value.ToString(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture));
                        }
                        catch (Exception ex)
                        {
                            mTokenizer.RaiseError(string.Format("Invalid number {0}", mTokenizer.value.ToString()));
                        }

                        mTokenizer.NextToken();
                        goto Foo;
                        break;
                    }

                case eTokenType.value_date:
                    {
                        try
                        {
                            ValueLeft = new opCodeImmediate(EvalType.Date, mTokenizer.value.ToString());
                        }
                        catch (Exception ex)
                        {
                            mTokenizer.RaiseError(string.Format("Invalid date {0}, it should be #DD/MM/YYYY hh:mm:ss#", mTokenizer.value.ToString()));
                        }

                        mTokenizer.NextToken();
                        goto Foo;
                        break;
                    }

                case eTokenType.open_parenthesis:
                    {
                        mTokenizer.NextToken();
                        ValueLeft = ParseExpr(null/* TODO Change to default(_) if this is not a reference type */, ePriority.none);
                        if (mTokenizer.type == eTokenType.close_parenthesis)
                        {
                            // good we eat the end parenthesis and continue ...
                            mTokenizer.NextToken();
                            goto Foo;
                        }
                        else
                            mTokenizer.RaiseUnexpectedToken("End parenthesis not found");
                        break;
                    }

                case eTokenType.operator_if:
                    {
                        // first check functions
                        IList<iEvalTypedValue> parameters = new List<iEvalTypedValue>();
                        mTokenizer.NextToken();
                        bool p = false;
                        parameters = ParseParameters(ref p);
                        goto Foo;
                        break;
                    }

                default:
                    {
                        goto Foo;
                        break;
                    }
            }
        }

        while (true)// parameters... 
;
        Foo:

        if (ValueLeft == null)
            mTokenizer.RaiseUnexpectedToken("No Expression found");
        ParseDot(ref ValueLeft);
        do
        {
            eTokenType tt;
            tt = mTokenizer.type;
            switch (tt)
            {
                case eTokenType.end_of_formula:
                    {
                        // end of line
                        return ValueLeft;
                    }

                case eTokenType.value_number:
                    {
                        mTokenizer.RaiseUnexpectedToken("Unexpected number without previous opterator");
                        break;
                    }

                case eTokenType.operator_plus:
                    {
                        if (priority < ePriority.plusminus)
                        {
                            mTokenizer.NextToken();
                            valueRight = ParseExpr(ValueLeft, ePriority.plusminus);
                            ValueLeft = new opCodeBinary(mTokenizer, ValueLeft, tt, valueRight);
                        }
                        else
                            goto Foo1;
                        break;
                    }

                case eTokenType.operator_minus:
                    {
                        if (priority < ePriority.plusminus)
                        {
                            mTokenizer.NextToken();
                            valueRight = ParseExpr(ValueLeft, ePriority.plusminus);
                            ValueLeft = new opCodeBinary(mTokenizer, ValueLeft, tt, valueRight);
                        }
                        else
                            goto Foo1;
                        break;
                    }

                case eTokenType.operator_concat:
                    {
                        if (priority < ePriority.concat)
                        {
                            mTokenizer.NextToken();
                            valueRight = ParseExpr(ValueLeft, ePriority.concat);
                            ValueLeft = new opCodeBinary(mTokenizer, ValueLeft, tt, valueRight);
                        }
                        else
                            goto Foo1;
                        break;
                    }

                case eTokenType.operator_mul:
                case eTokenType.operator_div:
                    {
                        if (priority < ePriority.muldiv)
                        {
                            mTokenizer.NextToken();
                            valueRight = ParseExpr(ValueLeft, ePriority.muldiv);
                            ValueLeft = new opCodeBinary(mTokenizer, ValueLeft, tt, valueRight);
                        }
                        else
                            goto Foo1;
                        break;
                    }

                case eTokenType.operator_percent:
                    {
                        if (priority < ePriority.percent)
                        {
                            mTokenizer.NextToken();
                            ValueLeft = new opCodeBinary(mTokenizer, ValueLeft, tt, Acc);
                        }
                        else
                            goto Foo1;
                        break;
                    }

                case eTokenType.operator_or:
                    {
                        if (priority < ePriority.or)
                        {
                            mTokenizer.NextToken();
                            valueRight = ParseExpr(ValueLeft, ePriority.or);
                            ValueLeft = new opCodeBinary(mTokenizer, ValueLeft, tt, valueRight);
                        }
                        else
                            goto Foo1;
                        break;
                    }

                case eTokenType.operator_and:
                    {
                        if (priority < ePriority.and)
                        {
                            mTokenizer.NextToken();
                            valueRight = ParseExpr(ValueLeft, ePriority.and);
                            ValueLeft = new opCodeBinary(mTokenizer, ValueLeft, tt, valueRight);
                        }
                        else
                            goto Foo1;
                        break;
                    }

                case eTokenType.operator_ne:
                case eTokenType.operator_gt:
                case eTokenType.operator_ge:
                case eTokenType.operator_eq:
                case eTokenType.operator_le:
                case eTokenType.operator_lt:
                    {
                        if (priority < ePriority.equality)
                        {
                            tt = mTokenizer.type;
                            mTokenizer.NextToken();
                            valueRight = ParseExpr(ValueLeft, ePriority.equality);
                            ValueLeft = new opCodeBinary(mTokenizer, ValueLeft, tt, valueRight);
                        }
                        else
                            goto Foo1;
                        break;
                    }

                default:
                    {
                        goto Foo1;
                        break;
                    }
            }
        }
        while (true);
        Foo1:
        return ValueLeft;
    }

    [Flags()]
    private enum eCallType
    {
        field = 1,
        method = 2,
        property = 4,
        all = 7
    }

    private bool EmitCallFunction(ref opCode valueLeft, string funcName, IList<iEvalTypedValue> parameters, eCallType CallType, bool ErrorIfNotFound)
    {
        opCode newOpcode = null;
        if (valueLeft == null)
        {
            object functions=null;
            
        
            foreach (var funcs in mEvaluator.mEnvironmentFunctionsList)
            {
                functions = funcs;
                while (functions != null)
                {
                    newOpcode = GetLocalFunction(funcs, funcs.GetType(), funcName, parameters, CallType);
                    if (newOpcode != null)
                        goto Foo;
                    if (funcs is iEvalFunctions)
                        functions = ((iEvalFunctions)funcs).InheritedFunctions();
                    else
                        break;
                }
                
            }

        }
        else
            newOpcode = GetLocalFunction(valueLeft, valueLeft.SystemType, funcName, parameters, CallType);

    Foo:
        if (newOpcode != null)
        {
            valueLeft = newOpcode;
            return true;
        }
        else
        {
            if (ErrorIfNotFound)
                mTokenizer.RaiseError("Variable or method " + funcName + " was not found");
            return false;
        }
    }

    private opCode GetLocalFunction(object @base, Type baseType, string funcName,IList<iEvalTypedValue> parameters, eCallType CallType)
    {
        MemberInfo mi;
        iEvalTypedValue var;
        mi = GetMemberInfo(baseType, funcName, parameters);
        if (mi != null)
        {
            switch (mi.MemberType)
            {
                case MemberTypes.Field:
                    {
                        if ((CallType & eCallType.field) == 0)
                            mTokenizer.RaiseError("Unexpected Field");
                        break;
                    }

                case MemberTypes.Method:
                    {
                        if ((CallType & eCallType.method) == 0)
                            mTokenizer.RaiseError("Unexpected Method");
                        break;
                    }

                case MemberTypes.Property:
                    {
                        if ((CallType & eCallType.property) == 0)
                            mTokenizer.RaiseError("Unexpected Property");
                        break;
                    }

                default:
                    {
                        mTokenizer.RaiseUnexpectedToken(mi.MemberType.ToString() + " members are not supported");
                        break;
                    }
            }

            return opCodeCallMethod.GetNew(mTokenizer, @base, mi, parameters);
        }
        if (@base is iVariableBag)
        {
            iEvalTypedValue val = ((iVariableBag)@base).GetVariable(funcName);
            if (val != null)
                return new opCodeGetVariable(val);
        }
        return null/* TODO Change to default(_) if this is not a reference type */;
    }

    private MemberInfo GetMemberInfo(Type objType, string func, IList<iEvalTypedValue> parameters)
    {
        System.Reflection.BindingFlags bindingAttr;
        bindingAttr = BindingFlags.GetProperty
                | BindingFlags.GetField
                | BindingFlags.Public
                | BindingFlags.InvokeMethod
                | BindingFlags.Instance
                | BindingFlags.Static;
        if (this.mEvaluator.CaseSensitive == false)
            bindingAttr = bindingAttr | BindingFlags.IgnoreCase;
        MemberInfo[] mis;

        if (func == null)
            mis = objType.GetDefaultMembers();
        else
            mis = objType.GetMember(func, bindingAttr);


        // There is a bit of cooking here...
        // lets find the most acceptable Member
        int Score, BestScore = 0;
        MemberInfo BestMember = null;
        ParameterInfo[] plist = null;
        int idx;

        MemberInfo mi;
        for (int i = 0; i <= mis.Length - 1; i++)
        {
            mi = mis[i];

            if (mi is MethodInfo)
                plist = ((MethodInfo)mi).GetParameters();
            else if (mi is PropertyInfo)
                plist = ((PropertyInfo)mi).GetIndexParameters();
            else if (mi is FieldInfo)
                plist = null;
            Score = 10; // by default
            idx = 0;
            if (plist == null)
                plist = new ParameterInfo[] { };
            if (parameters == null)
                parameters = new List<iEvalTypedValue>();

            System.Reflection.ParameterInfo pi;
            if (parameters.Count > plist.Length)
                Score = 0;
            else
                for (int index = 0; index <= plist.Length - 1; index++)
                {
                    pi = plist[index];
                    // For Each pi As Reflection.ParameterInfo In plist
                    if (idx < parameters.Count)
                        Score += ParamCompatibility(parameters[idx], pi.ParameterType);
                    else if (pi.IsOptional)
                        Score += 10;
                    else
                        // unknown parameter
                        Score = 0;
                    idx += 1;
                }
            if (Score > BestScore)
            {
                BestScore = Score;
                BestMember = mi;
            }
        }
        return BestMember;
    }

    private static int ParamCompatibility(object value, Type type)
    {
        // This function returns a score 1 to 10 to this question
        // Can this value fit into this type ?
        if (value == null)
        {
            if (type == typeof(object))
                return 10;
            if (type == typeof(string))
                return 8;
            return 5;
        }
        else if (type == value.GetType())
            return 10;
        else
            return 5;
    }

    private void ParseDot(ref opCode ValueLeft)
    {
        do
        {
            switch (mTokenizer.type)
            {
                case eTokenType.dot:
                    {
                        mTokenizer.NextToken();
                        break;
                    }

                case eTokenType.open_parenthesis:
                    {
                        break;
                    }

                default:
                    {
                        return;
                    }
            }

            ParseIdentifier(ref ValueLeft);
        }
        while (true);
    }

    private void ParseIdentifier(ref opCode ValueLeft)
    {
        // first check functions
        IList<iEvalTypedValue> parameters;   // parameters... 
        // Dim types As New ArrayList
        string func = mTokenizer.value.ToString();
        mTokenizer.NextToken();
        bool isBrackets = false;
        parameters = ParseParameters(ref isBrackets);
        if (parameters != null)
        {
            IList<iEvalTypedValue> EmptyParameters = new List<iEvalTypedValue>();
            bool ParamsNotUsed =false;
            if (mEvaluator.Syntax == eParserSyntax.Vb)
            {
                // in vb we don't know if it is array or not as we have only parenthesis
                // so we try with parameters first
                if (!EmitCallFunction(ref ValueLeft, func, parameters, eCallType.all, false))
                {
                    // and if not found we try as array or default member
                    EmitCallFunction(ref ValueLeft, func, EmptyParameters, eCallType.all, true);
                    ParamsNotUsed = true;
                }
            }
            else if (isBrackets)
            {
                if (!EmitCallFunction(ref ValueLeft, func, parameters, eCallType.property, false))
                {
                    EmitCallFunction(ref ValueLeft, func, EmptyParameters, eCallType.all, true);
                    ParamsNotUsed = true;
                }
            }
            else if (!EmitCallFunction(ref ValueLeft, func, parameters, eCallType.field | eCallType.method, ErrorIfNotFound: false))
            {
                EmitCallFunction(ref ValueLeft, func, EmptyParameters, eCallType.all, ErrorIfNotFound: true);
                ParamsNotUsed = true;
            }
            // we found a function without parameters 
            // so our parameters must be default property or an array
            Type t = ValueLeft.SystemType;
            if (ParamsNotUsed)
            {
                if (t.IsArray)
                {
                    if (parameters.Count == t.GetArrayRank())
                        ValueLeft = new opCodeGetArrayEntry(ValueLeft, parameters);
                    else
                        mTokenizer.RaiseError("This array has " + t.GetArrayRank() + " dimensions");
                }
                else
                {
                    MemberInfo mi;
                    mi = GetMemberInfo(t, null/* TODO Change to default(_) if this is not a reference type */, parameters);
                    if (mi != null)
                        ValueLeft = opCodeCallMethod.GetNew(mTokenizer, ValueLeft, mi, parameters);
                    else
                        mTokenizer.RaiseError("Parameters not supported here");
                }
            }
        }
        else
            EmitCallFunction(ref ValueLeft, func, parameters, eCallType.all, ErrorIfNotFound: true);
    }

    private IList<iEvalTypedValue> ParseParameters(ref bool brackets)
    {
        IList<iEvalTypedValue> parameters = new List<iEvalTypedValue>();
        opCode valueleft;
        eTokenType lClosing = eTokenType.none;

        if (mTokenizer.type == eTokenType.open_parenthesis || (mTokenizer.type == eTokenType.open_bracket & mEvaluator.Syntax == eParserSyntax.cSharp))
        {
            switch (mTokenizer.type)
            {
                case eTokenType.open_bracket:
                    {
                        lClosing = eTokenType.close_bracket;
                        brackets = true;
                        break;
                    }

                case eTokenType.open_parenthesis:
                    {
                        lClosing = eTokenType.close_parenthesis;
                        break;
                    }
            }

            mTokenizer.NextToken(); // eat the parenthesis
            do
            {
                if (mTokenizer.type == lClosing)
                {
                    // good we eat the end parenthesis and continue ...
                    mTokenizer.NextToken();
                    break;
                }

                valueleft = ParseExpr(null/* TODO Change to default(_) if this is not a reference type */, ePriority.none);
                parameters.Add(valueleft);
                if (mTokenizer.type == lClosing)
                {
                    // good we eat the end parenthesis and continue ...
                    mTokenizer.NextToken();
                    break;
                }
                else if (mTokenizer.type == eTokenType.comma)
                    mTokenizer.NextToken();
                else
                    mTokenizer.RaiseUnexpectedToken(lClosing.ToString() + " not found");
            }
            while (true);
        }
        return parameters;
    }
}

}
