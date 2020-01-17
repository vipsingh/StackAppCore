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
public class EvalVariable : iEvalTypedValue, iEvalHasDescription
{
    private object mValue;
    private string mDescription;
    private string mName;
    private System.Type mSystemType;
    private EvalType mEvalType;

    public string Description
    {
        get
        {
            return mDescription;
        }
    }

    public string Name
    {
        get
        {
            return mName;
        }
    }

    public object iEvalTypedValue_value
    {
        get
        {
            return mValue;
        }
    }

    public EvalType EvalType
    {
        get
        {
            return mEvalType;
        }
    }

    public System.Type SystemType
    {
        get
        {
            return mSystemType;
        }
    }


    public EvalVariable(string name, object originalValue, string description, System.Type systemType)
    {
        mName = name;
        mValue = originalValue;
        mDescription = description;
        mSystemType = systemType;
        mEvalType = Globals.GetEvalType(systemType);
    }

    public object Value
    {
        get
        {
            return mValue;
        }
        set
        {
            if (Value != mValue)
            {
                mValue = Value;
                ValueChanged.Invoke(this, new System.EventArgs());
            }
        }
    }

    public event ValueChangedHandler ValueChanged;

    //public delegate void ValueChangedEventHandler(object Sender, System.EventArgs e);
}

}
