using System;

namespace StackErp.Utils.ExpressionEval
{
    public interface iEvalFunctions
    {
        iEvalFunctions InheritedFunctions();
    }

    public interface iEvalHasDescription
    {
        string Name {get;}
        string Description {get;}
    }

    public delegate void ValueChangedHandler(Object Sender, EventArgs e);

    public interface iEvalValue
    {
        Object Value {get;}
        
        event ValueChangedHandler ValueChanged;
    }

    public interface iEvalTypedValue : iEvalValue
    {
         Type SystemType { get; }
        EvalType EvalType { get; }
    }

    public interface iVariableBag
    {
        iEvalTypedValue GetVariable(string varname);
    }
}
