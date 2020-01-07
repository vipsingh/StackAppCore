using System;
using System.Collections.Generic;
using StackErp.Model.Entity;

namespace StackErp.Model.Entity
{
    public class FieldDataCollection: Dictionary<string, FieldData>
    {

    }
    public class FieldData
    {
        public BaseField Field { get; }
        public object Value {set;get;}
        public string Text {set;get;}
        public string Code {set;get;}
        public bool IsValid {private set;get;}
        public bool IsChanged {set;get;}

        public FieldData(BaseField field, object defaultValue = null)
        {
            Field = field;
            Value = defaultValue;
        }

        public void SetValue(object value)
        {
            bool isValid = false;
            Value = Field.ResolveSetValue(value, out isValid);

            IsChanged = true;
            IsValid = isValid;
        }

        public override string ToString()
        {
            if (IsValid)
            {
                return Convert.ToString(Value);
            }

            return null;
        }

        
    }
}