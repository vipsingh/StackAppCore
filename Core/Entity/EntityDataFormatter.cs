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
        public object FormatData(FormatInfo formatInfo, object value)
        {
            if (value == null) return value;

            if (formatInfo.FieldBaseType == BaseTypeCode.DateTime || formatInfo.FieldBaseType == BaseTypeCode.Date)
            {
                return this.FormatDate(value);
            }

            return value;
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

        public string FormatNumeric(object value, int precesion = 2)
        {                                                   
            var v = Convert.ToDouble(value.ToString());

            return String.Format(CultureInfo.InvariantCulture, "{0:0.00}", v);
        }
    }
}