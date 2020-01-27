using System;
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

            if(field.BaseType == BaseTypeCode.Int32 || field.BaseType == BaseTypeCode.Int64 || field.BaseType == BaseTypeCode.Int16)
            {
                if(Convert.ToInt64(val) == 0)
                    return false;
            } 
            else if(field.BaseType == BaseTypeCode.Boolean)
            {
                if(!Convert.ToBoolean(val))
                    return false;
            } 
            else if(field.BaseType == BaseTypeCode.Decimal || field.BaseType == BaseTypeCode.Double)
            {
                if(!field.AllowZero && Convert.ToDecimal(val) == 0)
                    return false;
            }

            return true;
        }
    }
}
