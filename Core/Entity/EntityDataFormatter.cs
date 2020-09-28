using System;
using System.Globalization;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.Core.Entity
{
    public class EntityDataFormatter : IEntityDataFormatter
    {
        private StackAppContext _AppContext;
        public EntityDataFormatter(StackAppContext appContext)
        {
            _AppContext = appContext;
        }
        public string FormatData(FormatInfo formatInfo, object value)
        {
            if (value == null) return null;

            if (formatInfo.FieldBaseType == TypeCode.DateTime)
            {
                return this.FormatDate(value, formatInfo.FormatString);
            }
            else if (formatInfo.FieldBaseType == TypeCode.Decimal)
            {
                return this.FormatNumeric(value, formatInfo.FormatString, formatInfo.DecimalPlace);
            }

            return value.ToString();
        }

        public string FormatDate(object value, string format = "")
        {
                if (string.IsNullOrEmpty(format))
                    format = _AppContext.ShortDateFormat;

                if(value is DateTime) {
                    var v = (DateTime)value;
                    if (v != DateTime.MinValue)
                    {
                        return v.ToString(format);
                    }
                    return null;
                } 
                else 
                {
                    return DateTime.Parse(value.ToString()).ToString(format);
                }
                
        }

        public string FormatNumeric(object value, string format = "{0:0.00}", int precesion = 2)
        {                                                   
            var v = Convert.ToDouble(value.ToString());

            return String.Format(CultureInfo.InvariantCulture, format, v);
        }
    }
}