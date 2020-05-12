using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.Core.Form
{
    public class DataValidator
    {
        public void Validate()
        {

        }

        public bool HasValue(BaseField field, object val)
        {
            if(val == null || (val is string && val.ToString() == ""))
                return false;
            if (val is ICollection) {
                if (((ICollection)val).Count == 0)
                    return false;
                else
                    return true;
            } else {
                
                if(field.BaseType == TypeCode.Int32 || field.BaseType == TypeCode.Int64 || field.BaseType == TypeCode.Int16)
                {
                    if(Convert.ToInt64(val) == 0)
                        return false;
                } 
                else if(field.BaseType == TypeCode.Boolean)
                {
                    if(!Convert.ToBoolean(val))
                        return false;
                } 
                else if(field.BaseType == TypeCode.Decimal || field.BaseType == TypeCode.Double)
                {
                    if(!field.AllowZero && Convert.ToDecimal(val) == 0)
                        return false;
                }
            }

            return true;
        }
    }
}
