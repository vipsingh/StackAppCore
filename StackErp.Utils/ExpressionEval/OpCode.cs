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


public abstract class opCode : iEvalTypedValue, iEvalHasDescription
{
    protected ValueDelegate mValueDelegate;
    protected delegate object ValueDelegate();

    delegate void RunDelegate();

    protected opCode()
    {
    }

    protected void RaiseEventValueChanged(object sender, EventArgs e)
    {
        ValueChanged.Invoke(sender, e);
    }

    public abstract EvalType EvalType { get; }

    public bool CanReturn(EvalType type)
    {
        return true;
    }

    public virtual string Description
    {
        get
        {
            return "opCode " + this.GetType().Name;
        }
    }

    public virtual string Name
    {
        get
        {
            return "opCode " + this.GetType().Name;
        }
    }

    public virtual object Value
    {
        get
        {
            return mValueDelegate();
        }
    }

    public virtual System.Type SystemType
    {
        get
        {
            return Globals.GetSystemType(this.EvalType);
        }
    }


    protected internal void Convert(tokenizer tokenizer, ref opCode param1, EvalType EvalType)
    {
        if (param1.EvalType != EvalType)
        {
            if (param1.CanReturn(EvalType))
                param1 = new opCodeConvert(tokenizer, param1, EvalType);
            else
                tokenizer.RaiseError("Cannot convert " + param1.Name + " into " + EvalType);
        }
    }

    protected static void ConvertToSystemType(ref iEvalTypedValue param1, Type SystemType)
    {
        if (param1.SystemType != SystemType)
        {
            if (SystemType == typeof(object))
            {
            }
            else
                param1 = new opCodeSystemTypeConvert(param1, SystemType);
        }
    }

    protected void SwapParams(ref opCode Param1, ref opCode Param2)
    {
        opCode swp = Param1;
        Param1 = Param2;
        Param2 = swp;
    }

    public event ValueChangedHandler ValueChanged;

    //public delegate void ValueChangedEventHandler(object Sender, System.EventArgs e);
}

internal class opCodeImmediate : opCode
{
    private object mValue;
    private EvalType mEvalType;

    public opCodeImmediate(EvalType EvalType, object value)
    {
        mEvalType = EvalType;
        mValue = value;
    }

    public override object Value
    {
        get
        {
            return mValue;
        }
    }


    public override EvalType EvalType
    {
        get
        {
            return mEvalType;
        }
    }
}

public class opCodeVariable: opCode
{
    EvalVariable mVariable;
     public opCodeVariable(EvalVariable variable)
    {
        mVariable = variable;
        mVariable.ValueChanged += mVariable_ValueChanged;
    }
    public override object Value
    {
        get
        {
            return mVariable;
        }
    }

    public override EvalType EvalType
    {
        get
        {
            return mVariable.EvalType;
        }
    }

    private void mVariable_ValueChanged(object Sender, System.EventArgs e)
    {
        base.RaiseEventValueChanged(Sender, e);
    }
}

internal class opCodeUnary : opCode
{
    private opCode mParam1;
    private EvalType mEvalType;

    public opCodeUnary(eTokenType tt, opCode param1)
    {
        mParam1 = param1;
        mParam1.ValueChanged += mParam1_ValueChanged;
        EvalType v1Type = mParam1.EvalType;

        switch (tt)
        {
            case eTokenType.operator_not:
                {
                    if (v1Type == EvalType.Boolean)
                    {
                        mValueDelegate = BOOLEAN_NOT;
                        mEvalType = EvalType.Boolean;
                    }

                    break;
                }

            case eTokenType.operator_minus:
                {
                    if (v1Type == EvalType.Number)
                    {
                        mValueDelegate = NUM_CHGSIGN;
                        mEvalType = EvalType.Number;
                    }

                    break;
                }
        }
    }

    private object BOOLEAN_NOT()
    {
        return !(bool)mParam1.Value;
    }

    private object NUM_CHGSIGN()
    {
        return -(double)mParam1.Value;
    }

    public override EvalType EvalType
    {
        get
        {
            return mEvalType;
        }
    }

    private void mParam1_ValueChanged(object Sender, System.EventArgs e)
    {
        base.RaiseEventValueChanged(Sender, e);
    }
}
internal class opCodeConvert : opCode
{
    private iEvalTypedValue mParam1;
    private EvalType mEvalType = EvalType.Unknown;

    public opCodeConvert(tokenizer tokenizer, iEvalTypedValue param1, EvalType EvalType)
    {
        mParam1 = param1;
        mParam1.ValueChanged += mParam1_ValueChanged;
        switch (EvalType)
        {
            case EvalType.Boolean:
                {
                    mValueDelegate = TBool;
                    mEvalType = EvalType.Boolean;
                    break;
                }

            case EvalType.Date:
                {
                    mValueDelegate = TDate;
                    mEvalType = EvalType.Date;
                    break;
                }

            case EvalType.Number:
                {
                    mValueDelegate = TNum;
                    mEvalType = EvalType.Number;
                    break;
                }

            case EvalType.String:
                {
                    mValueDelegate = TStr;
                    mEvalType = EvalType.String;
                    break;
                }

            default:
                {
                    tokenizer.RaiseError("Cannot convert " + param1.SystemType.Name + " to " + EvalType);
                    break;
                }
        }
    }

    private object TBool()
    {
        return Globals.TBool(mParam1);
    }

    private object TDate()
    {
        return Globals.TDate(mParam1);
    }

    private object TNum()
    {
        return Globals.TNum(mParam1);
    }

    private object TStr()
    {
        return Globals.TStr(mParam1);
    }

    public override EvalType EvalType
    {
        get
        {
            return mEvalType;
        }
    }

    private void mParam1_ValueChanged(object Sender, System.EventArgs e)
    {
        base.RaiseEventValueChanged(Sender, e);
    }
}

internal class opCodeSystemTypeConvert : opCode
{
    private iEvalTypedValue mParam1;
    private EvalType mEvalType = EvalType.Unknown;
    private System.Type mSystemType;

    public opCodeSystemTypeConvert(iEvalTypedValue param1, System.Type Type)
    {
        mParam1 = param1;
        mParam1.ValueChanged += mParam1_ValueChanged;
        mValueDelegate = CType;
        mSystemType = Type;
        mEvalType = Globals.GetEvalType(Type);
    }

    private object CType()
    {
        return System.Convert.ChangeType(mParam1.Value, mSystemType);
    }

    public override EvalType EvalType
    {
        get
        {
            return mEvalType;
        }
    }

    public override System.Type SystemType
    {
        get
        {
            return mSystemType;
        }
    }

    private void mParam1_ValueChanged(object Sender, System.EventArgs e)
    {
        base.RaiseEventValueChanged(Sender, e);
    }
}

internal class opCodeBinary : opCode
{
    private opCode mParam1;
    private opCode mParam2;
    private EvalType mEvalType;

    public opCodeBinary(tokenizer tokenizer, opCode param1, eTokenType tt, opCode param2)
    {
        mParam1 = param1;
        mParam2 = param2;

        mParam1.ValueChanged += mParam1_ValueChanged;
        mParam2.ValueChanged += mParam2_ValueChanged;

        EvalType v1Type = mParam1.EvalType;
        EvalType v2Type = mParam2.EvalType;

        switch (tt)
        {
            case eTokenType.operator_plus:
                {
                    if (v1Type == EvalType.Number & v2Type == EvalType.Number)
                    {
                        mValueDelegate = NUM_PLUS_NUM;
                        mEvalType = EvalType.Number;
                    }
                    else if (v1Type == EvalType.Number & v2Type == EvalType.Date)
                    {
                        SwapParams(ref mParam1, ref mParam2);
                        mValueDelegate = DATE_PLUS_NUM;
                        mEvalType = EvalType.Date;
                    }
                    else if (v1Type == EvalType.Date & v2Type == EvalType.Number)
                    {
                        mValueDelegate = DATE_PLUS_NUM;
                        mEvalType = EvalType.Date;
                    }
                    else if (mParam1.CanReturn(EvalType.String) & mParam2.CanReturn(EvalType.String))
                    {
                        Convert(tokenizer, ref param1, EvalType.String);
                        mValueDelegate = STR_CONCAT_STR;
                        mEvalType = EvalType.String;
                    }

                    break;
                }

            case eTokenType.operator_minus:
                {
                    if (v1Type == EvalType.Number & v2Type == EvalType.Number)
                    {
                        mValueDelegate = NUM_MINUS_NUM;
                        mEvalType = EvalType.Number;
                    }
                    else if (v1Type == EvalType.Date & v2Type == EvalType.Number)
                    {
                        mValueDelegate = DATE_MINUS_NUM;
                        mEvalType = EvalType.Date;
                    }
                    else if (v1Type == EvalType.Date & v2Type == EvalType.Date)
                    {
                        mValueDelegate = DATE_MINUS_DATE;
                        mEvalType = EvalType.Number;
                    }

                    break;
                }

            case eTokenType.operator_mul:
                {
                    if (v1Type == EvalType.Number & v2Type == EvalType.Number)
                    {
                        mValueDelegate = NUM_MUL_NUM;
                        mEvalType = EvalType.Number;
                    }

                    break;
                }

            case eTokenType.operator_div:
                {
                    if (v1Type == EvalType.Number & v2Type == EvalType.Number)
                    {
                        mValueDelegate = NUM_DIV_NUM;
                        mEvalType = EvalType.Number;
                    }

                    break;
                }

            case eTokenType.operator_percent:
                {
                    if (v1Type == EvalType.Number & v2Type == EvalType.Number)
                    {
                        mValueDelegate = NUM_PERCENT_NUM;
                        mEvalType = EvalType.Number;
                    }

                    break;
                }

            case eTokenType.operator_and:
            case eTokenType.operator_or:
                {
                    Convert(tokenizer, ref mParam1, EvalType.Boolean);
                    Convert(tokenizer, ref mParam2, EvalType.Boolean);
                    switch (tt)
                    {
                        case eTokenType.operator_or:
                            {
                                mValueDelegate = BOOL_OR_BOOL;
                                mEvalType = EvalType.Boolean;
                                break;
                            }

                        case eTokenType.operator_and:
                            {
                                mValueDelegate = BOOL_AND_BOOL;
                                mEvalType = EvalType.Boolean;
                                break;
                            }
                    }

                    break;
                }
        }

        if (mValueDelegate == null)
            tokenizer.RaiseError("Cannot apply the operator " + tt.ToString().Replace("operator_", "") + " on " + v1Type.ToString() + " and " + v2Type.ToString());
    }

    private object BOOL_AND_BOOL()
    {
        return (bool)mParam1.Value & (bool)mParam2.Value;
    }

    private object BOOL_OR_BOOL()
    {
        return (bool)mParam1.Value | (bool)mParam2.Value;
    }

    private object BOOL_XOR_BOOL()
    {
        return (bool)mParam1.Value ^ (bool)mParam2.Value;
    }

    private object NUM_EQ_NUM()
    {
        return (double)mParam1.Value == (double)mParam2.Value;
    }

    private object NUM_LT_NUM()
    {
        return (double)mParam1.Value < (double)mParam2.Value;
    }

    private object NUM_GT_NUM()
    {
        return (double)mParam1.Value > (double)mParam2.Value;
    }

    private object NUM_GE_NUM()
    {
        return (double)mParam1.Value >= (double)mParam2.Value;
    }

    private object NUM_LE_NUM()
    {
        return (double)mParam1.Value <= (double)mParam2.Value;
    }

    private object NUM_NE_NUM()
    {
        return (double)mParam1.Value != (double)mParam2.Value;
    }

    private object NUM_PLUS_NUM()
    {
        return (double)mParam1.Value + (double)mParam2.Value;
    }

    private object NUM_MUL_NUM()
    {
        return (double)mParam1.Value * (double)mParam2.Value;
    }

    private object NUM_MINUS_NUM()
    {
        return (double)mParam1.Value - (double)mParam2.Value;
    }

    private object DATE_PLUS_NUM()
    {
        return ((DateTime)mParam1.Value).AddDays((double)mParam2.Value);
    }

    private object DATE_MINUS_DATE()
    {
        return ((DateTime)mParam1.Value).Subtract((DateTime)mParam2.Value).TotalDays;
    }

    private object DATE_MINUS_NUM()
    {
        return ((DateTime)mParam1.Value).AddDays(-(double)mParam2.Value);
    }

    private object STR_CONCAT_STR()
    {
        return mParam1.Value.ToString() + mParam2.Value.ToString();
    }

    private object NUM_DIV_NUM()
    {
        return (double)mParam1.Value / (double)mParam2.Value;
    }

    private object NUM_PERCENT_NUM()
    {
        return (double)mParam2.Value * ((double)mParam1.Value / 100);
    }

    public override EvalType EvalType
    {
        get
        {
            return mEvalType;
        }
    }

    private void mParam1_ValueChanged(object Sender, System.EventArgs e)
    {
        base.RaiseEventValueChanged(Sender, e);
    }

    private void mParam2_ValueChanged(object Sender, System.EventArgs e)
    {
        base.RaiseEventValueChanged(Sender, e);
    }
}

public class opCodeGetVariable : opCode
{
    private iEvalTypedValue mParam1;

    public opCodeGetVariable(iEvalTypedValue value)
    {
        mParam1 = value;
        mParam1.ValueChanged += mParam1_ValueChanged;
    }


    public override object Value
    {
        get
        {
            return mParam1.Value;
        }
    }

    public override System.Type SystemType
    {
        get
        {
            return mParam1.SystemType;
        }
    }

    public override EvalType EvalType
    {
        get
        {
            return mParam1.EvalType;
        }
    }

    private void mParam1_ValueChanged(object Sender, System.EventArgs e)
    {
        base.RaiseEventValueChanged(Sender, e);
    }
}

public class opCodeCallMethod : opCode
{
    private object mBaseObject;
    private System.Type mBaseSystemType;
    private EvalType mBaseEvalType;
    private iEvalValue mBaseValue;  // for the events only
    private object mBaseValueObject;

    private System.Reflection.MemberInfo mMethod;
    private iEvalTypedValue[] mParams;
    private object[] mParamValues;

    private System.Type mResultSystemType;
    private EvalType mResultEvalType;
    private iEvalValue mResultValue;  // just for some

    internal opCodeCallMethod(object baseObject, System.Reflection.MemberInfo method, IList<iEvalTypedValue> @params)
    {
        if (@params == null)
            @params = new iEvalTypedValue[] { };
        iEvalTypedValue[] newParams = new iEvalTypedValue[@params.Count - 1 + 1];
        object[] newParamValues = new object[@params.Count - 1 + 1];

        @params.CopyTo(newParams, 0);

        foreach (iEvalTypedValue p in newParams)
            p.ValueChanged += mParamsValueChanged;
        mParams = newParams;
        mParamValues = newParamValues;        
        mBaseObject = baseObject;
        mMethod = method;

        if (mBaseObject is iEvalValue)
        {
            if (mBaseObject is iEvalTypedValue)
            {
                {
                    var withBlock = (iEvalTypedValue)mBaseObject;
                    mBaseSystemType = withBlock.SystemType;
                    mBaseEvalType = withBlock.EvalType;
                }
            }
            else
            {
                mBaseSystemType = mBaseObject.GetType();
                mBaseEvalType = Globals.GetEvalType(mBaseSystemType);
            }
        }
        else
        {
            mBaseSystemType = mBaseObject.GetType();
            mBaseEvalType = Globals.GetEvalType(mBaseSystemType);
        }

        System.Reflection.ParameterInfo[] paramInfo = null;
        if (method is System.Reflection.PropertyInfo)
        {
            {
                var withBlock = (System.Reflection.PropertyInfo)method;
                mResultSystemType = withBlock.PropertyType;
                paramInfo = withBlock.GetIndexParameters();
            }
            mValueDelegate = GetProperty;
        }
        else if (method is System.Reflection.MethodInfo)
        {
            {
                var withBlock = (System.Reflection.MethodInfo)method;
                mResultSystemType = withBlock.ReturnType;
                paramInfo = withBlock.GetParameters();
            }
            mValueDelegate = GetMethod;
        }
        else if (method is System.Reflection.FieldInfo)
        {
            {
                var withBlock = (System.Reflection.FieldInfo)method;
                mResultSystemType = withBlock.FieldType;
                paramInfo = new System.Reflection.ParameterInfo[] { };
            }
            mValueDelegate = GetField;
        }

        for (int i = 0; i <= mParams.Length - 1; i++)
        {
            if (i < paramInfo.Length)
                ConvertToSystemType(ref mParams[i], paramInfo[i].ParameterType);
        }

        if (typeof(iEvalValue).IsAssignableFrom(mResultSystemType))
        {
            mResultValue = (iEvalValue)InternalValue();
            if (mResultValue is iEvalTypedValue)
            {
                {
                    var withBlock = (iEvalTypedValue)mResultValue;
                    mResultSystemType = withBlock.SystemType;
                    mResultEvalType = withBlock.EvalType;
                }
            }
            else if (mResultValue == null)
            {
                mResultSystemType = typeof(object);
                mResultEvalType = EvalType.Object;
            }
            else
            {
                object v = mResultValue.Value;
                if (v == null)
                {
                    mResultSystemType = typeof(object);
                    mResultEvalType = EvalType.Object;
                }
                else
                {
                    mResultSystemType = v.GetType();
                    mResultEvalType = Globals.GetEvalType(mResultSystemType);
                }
            }
        }
        else
        {
            mResultSystemType = SystemType;
            mResultEvalType = Globals.GetEvalType(SystemType);
        }
        if(mBaseValue!= null)
        mBaseValue.ValueChanged += mBaseVariable_ValueChanged;
        if (mResultValue != null)
        mResultValue.ValueChanged += mResultVariable_ValueChanged;
    }

    protected internal static opCode GetNew(tokenizer tokenizer, object baseObject, System.Reflection.MemberInfo method, IList<iEvalTypedValue> @params)
    {
        opCode o;
        o = new opCodeCallMethod(baseObject, method, @params);

        if (o.EvalType != EvalType.Object && o.SystemType != Globals.GetSystemType(o.EvalType))
            return new opCodeConvert(tokenizer, o, o.EvalType);
        else
            return o;
    }

    private object GetProperty()
    {
        object res = ((System.Reflection.PropertyInfo)mMethod).GetValue(mBaseValueObject, mParamValues);
        return res;
    }

    private object GetMethod()
    {
        object res = ((System.Reflection.MethodInfo)mMethod).Invoke(mBaseValueObject, mParamValues);
        return res;
    }

    private object GetField()
    {
        object res = ((System.Reflection.FieldInfo)mMethod).GetValue(mBaseValueObject);
        return res;
    }

    private object InternalValue()
    {
        for (int i = 0; i <= mParams.Length - 1; i++)
            mParamValues[i] = mParams[i].Value;
        if (mBaseObject is iEvalValue)
        {
            mBaseValue = (iEvalValue)mBaseObject;
            mBaseValueObject = mBaseValue.Value;
        }
        else
            mBaseValueObject = mBaseObject;
        return base.mValueDelegate();
    }

    public override object Value
    {
        get
        {
            object res = InternalValue();
            if (res is iEvalValue)
            {
                mResultValue = (iEvalValue)res;
                res = mResultValue.Value;
            }
            return res;
        }
    }

    public override System.Type SystemType
    {
        get
        {
            return mResultSystemType;
        }
    }

    public override EvalType EvalType
    {
        get
        {
            return mResultEvalType;
        }
    }

    private void mParamsValueChanged(object Sender, System.EventArgs e)
    {
        base.RaiseEventValueChanged(Sender, e);
    }

    private void mBaseVariable_ValueChanged(object Sender, System.EventArgs e)
    {
        base.RaiseEventValueChanged(Sender, e);
    }

    private void mResultVariable_ValueChanged(object Sender, System.EventArgs e)
    {
        base.RaiseEventValueChanged(Sender, e);
    }
}

public class opCodeGetArrayEntry : opCode
{
    private opCode mArray;

    private iEvalTypedValue[] mParams;
    private int[] mValues;
    private EvalType mResultEvalType;
    private System.Type mResultSystemType;

    public opCodeGetArrayEntry(opCode array, IList<iEvalTypedValue> @params)
    {
        iEvalTypedValue[] newParams = new iEvalTypedValue[@params.Count - 1 + 1];
        int[] newValues = new int[@params.Count - 1 + 1];
        @params.CopyTo(newParams, 0);
        mArray = array;
        mArray.ValueChanged += mBaseVariable_ValueChanged;
        mParams = newParams;
        mValues = newValues;
        mResultSystemType = array.SystemType.GetElementType();
        mResultEvalType = Globals.GetEvalType(mResultSystemType);
    }

    public override object Value
    {
        get
        {
            object res;
            Array arr = (Array)mArray.Value;
            for (int i = 0; i <= mValues.Length - 1; i++)
                mValues[i] = System.Convert.ToInt32(mParams[i].Value);
            res = arr.GetValue(mValues);
            return res;
        }
    }

    public override System.Type SystemType
    {
        get
        {
            return mResultSystemType;
        }
    }

    public override EvalType EvalType
    {
        get
        {
            return mResultEvalType;
        }
    }

    private void mBaseVariable_ValueChanged(object Sender, System.EventArgs e)
    {
        base.RaiseEventValueChanged(Sender, e);
    }
}

}
