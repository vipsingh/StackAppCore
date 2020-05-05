using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StackErp.Model
{
    public static class DataHelper
    {
        
        public static int ToInt(object input, int valueIfNull)
        {
            int ret = valueIfNull;

            try
            {
                if (input != null)
                {
                    var valType = input.GetType();

                    if (valType.IsEnum)
                        ret = Convert.ToInt32(input);
                    else if (!Int32.TryParse(input.ToString(), out ret))
                        ret = valueIfNull;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return ret;
        }
        public static bool ToBool(object input, bool valueIfNull)
        {
            if (input == null || input == DBNull.Value)
                return valueIfNull;

            return GetBooleanFlag(input.ToString());
        }

        public static bool GetBooleanFlag(string val)
        {
            bool obj = false;

            if (!string.IsNullOrWhiteSpace(val))
            {
                val = val.Trim().ToLower();
                if (val == "1" || val == "t" || val == "true" || val == "y" || val == "yes" || val == "ok")
                {
                    obj = true;
                }
            }

            return obj;
        }
        public static object GetNativeData(string value, object defaultData)
        {
            if (defaultData == null)
                return value;

            var typeCode = Type.GetTypeCode(defaultData.GetType());


            return GetDataValue(value, typeCode);

        }
        public static T GetData<T>(object value, T defaultValue)
        {
            if (value == null)
                return defaultValue;

            var typeCode = Type.GetTypeCode(typeof(T));

            if (typeof(T) == value.GetType())
                return (T)value;


            object objectValue = GetDataValue(value, typeCode);


            return (T)objectValue;// Convert.ChangeType(objectValue, typeof(T));
        }

        internal static object GetDataValue(object value, Type fieldType)
        {
            if (value.GetType() == fieldType)
                return value;

            if (fieldType == typeof(Guid))
                return new Guid(value.ToString());

            if (fieldType.IsEnum)
            {
                if (!string.IsNullOrWhiteSpace(value.ToString()))
                    return Enum.Parse(fieldType, value.ToString().Replace(" ",""));
            }

            return GetDataValue(value, Type.GetTypeCode(fieldType));
        }
        
        public static object GetDataValue(object value, TypeCode typeCode)
        {
            if (value == null)
            {
                return null;
            }

            object objectValue = null;
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    objectValue = GetBooleanFlag(value.ToString());
                    break;
                case TypeCode.DateTime:
                    var dt = DateTime.MinValue;

                    if (value is DateTime)
                        dt = (DateTime)value;
                    else
                        dt = ParseCurrentDate(value.ToString());

                    objectValue = dt;

                    break;

                case TypeCode.Decimal:
                    if (value is decimal)
                    {
                        objectValue = value;
                    }
                    else
                    {
                        decimal doubleValue;
                        decimal.TryParse(value.ToString(), out doubleValue);

                        objectValue = doubleValue;
                    }
                    break;
                case TypeCode.Double:
                    if (value is double)
                    {
                        objectValue = value;
                    }
                    else
                    {
                        double doubleValue;
                        double.TryParse(value.ToString(), out doubleValue);

                        objectValue = doubleValue;
                    }
                    break;
                case TypeCode.Int64:

                    if (value is Int64)
                    {
                        objectValue = value;
                    }
                    else if (value is Int32 || value is Int16)
                    {
                        objectValue = Convert.ToInt64(value);
                    }
                    else
                    {
                        long longValue;
                        Int64.TryParse(value.ToString(), out longValue);

                        objectValue = longValue;
                    }


                    break;
                case TypeCode.Int32:
                    if (value is Int32 )
                        objectValue = value;
                    else if (value is Int16 || value is decimal)
                    {
                        objectValue = Convert.ToInt32(value);
                    }
                    else
                    {
                        var valType = value.GetType();
                        int number = 0;

                        if (valType.IsEnum)
                            number = Convert.ToInt32(value);
                        else
                            int.TryParse(value.ToString(), out number);

                        objectValue = number;
                    }
                    break;
                case TypeCode.String:
                    objectValue = value.ToString();
                    break;
                default:
                    objectValue = value;
                    break;
            }
            return objectValue;
        }

        public static bool ParseDateTime(string dateValue, string[] formats, DateTimeFormatInfo formatInfo, out DateTime result)
        {
            result = DateTime.MinValue;

            if (string.IsNullOrEmpty(dateValue))
                return false;

            if (DateTime.TryParseExact(dateValue, formats, formatInfo,
                           DateTimeStyles.AllowWhiteSpaces, out result))
            {
                return true;
            }

            return false;
        }

        public static DateTime ParseCurrentDate(string dateValue)
        {
            DateTime date = DateTime.MinValue;
            ParseDateTime(dateValue, GetParseDateFormats(true), DateTimeFormatInfo.CurrentInfo, out date);
            return date;
        }
        public static string[] GetParseDateFormats(bool isCurrentInfo)
        {
            string format = DateTimeFormatInfo.InvariantInfo.ShortDatePattern;

            if (isCurrentInfo)
            {
                format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
            }
            else
            {
                format = DateTimeFormatInfo.InvariantInfo.ShortDatePattern;
            }

            ArrayList list = new ArrayList();
            list.Add(format);

            if (format == "s")
                format = "";

            list.Add(format + " H:mm");
            list.Add(format + " h:mm:ss tt");
            list.Add(format + " HH:mm:ss");
            list.Add(format + " HH:mm");
            list.Add(format + " h:mm tt");
            list.Add(format + " HH:mm:ss.fff");

            return (string[])(list.ToArray(typeof(string)));
        }
        public static List<int> ConvertToIntList(string[] list)
        {
            if (list == null)
                return null;

            List<int> numberList = new List<int> ();

            foreach (var item in list)
            {
                numberList.Add(ToInt(item, -1));
            }

            return numberList;
        }

        public static bool IsValid(object value, TypeCode typeCode, dynamic threashHold)
        {
            switch (typeCode)
            {
                case TypeCode.DateTime:
                    return (DateTime)value != DateTime.MinValue;
                case TypeCode.Decimal:
                    if (threashHold == null)
                    {
                        threashHold = 1;
                    }
                    return (decimal)value >= (decimal)threashHold;
                case TypeCode.Int16:
                    if (threashHold == null)
                    {
                        threashHold = 1;
                    }
                    return (short)value >= (short)threashHold;
                case TypeCode.Int32:
                    if (threashHold == null)
                    {
                        threashHold = 1;
                    }
                    return ToInt(value, 0) >= (int)threashHold;
                case TypeCode.Double:
                    if (threashHold == null)
                    {
                        threashHold = 1;
                    }
                    return (Double)value >= (Double)threashHold;
                case TypeCode.Int64:
                    if (threashHold == null)
                    {
                        threashHold = 1;
                    }
                    return (long)value >= (long)threashHold;
                default:
                    return !string.IsNullOrWhiteSpace( value.ToString());
            }
        }
    }
}