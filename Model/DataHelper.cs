using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
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
            if (value.GetType() == typeof(JValue)) {
                value = ((JValue)value).Value;
            }

            if (typeof(T) == value.GetType())
                return (T)value;


            object objectValue = GetDataValue(value, typeCode);


            return (T)objectValue;// Convert.ChangeType(objectValue, typeof(T));
        }

        public static object GetDataValue(object value, Type fieldType)
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
                case TypeCode.Int16:
                    if (value is Int16 )
                        objectValue = value;
                    else if (value is Int32 || value is decimal)
                    {
                        objectValue = Convert.ToInt16(value);
                    }
                    else
                    {
                        var valType = value.GetType();
                        Int16 number = 0;

                        if (valType.IsEnum)
                            number = Convert.ToInt16(value);
                        else
                            Int16.TryParse(value.ToString(), out number);

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

        public static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        public static object ResolveWidgetValue(object postValue, TypeCode tCode = TypeCode.Int32)
        {
            if(postValue is JObject)
            {
                var v = (JObject)postValue;
                return DataHelper.GetDataValue(v["Value"], tCode);
                
            } else if (postValue is JArray) {
                var postVals = new List<int>();
                foreach(var o in (JArray)postValue) {
                    if (o is JObject)
                        postVals.Add((int)DataHelper.GetDataValue(o["Value"], tCode));
                    else
                        postVals.Add((int)DataHelper.GetDataValue(o, tCode));
                }
                return postVals;
            }
            else {
                return DataHelper.GetDataValue(postValue, tCode);
            }
        }

        public static string EnsureNumericOnly(string str)
        {
            return string.IsNullOrEmpty(str) ? string.Empty : new string(str.Where(char.IsDigit).ToArray());
        }

        public static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            //also see Enumerable.SequenceEqual(a1, a2);
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            var comparer = EqualityComparer<T>.Default;
            return !a1.Where((t, i) => !comparer.Equals(t, a2[i])).Any();
        }

        /// <summary>
        /// Sets a property on an object to a value.
        /// </summary>
        /// <param name="instance">The object whose property to set.</param>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <param name="value">The value to set the property to.</param>
        public static void SetProperty(object instance, string propertyName, object value)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            var instanceType = instance.GetType();
            var pi = instanceType.GetProperty(propertyName);
            if (pi == null)
                throw new Exception(string.Format("No property '{0}' found on the instance of type '{1}'.", propertyName, instanceType));
            if (!pi.CanWrite)
                throw new Exception(string.Format("The property '{0}' on the instance of type '{1}' does not have a setter.", propertyName, instanceType));
            if (value != null && !value.GetType().IsAssignableFrom(pi.PropertyType))
                value = To(value, pi.PropertyType);
            pi.SetValue(instance, value, Array.Empty<object>());
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType)
        {
            return To(value, destinationType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <param name="culture">Culture</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value == null) 
                return null;

            var sourceType = value.GetType();

            var destinationConverter = TypeDescriptor.GetConverter(destinationType);
            if (destinationConverter.CanConvertFrom(value.GetType()))
                return destinationConverter.ConvertFrom(null, culture, value);

            var sourceConverter = TypeDescriptor.GetConverter(sourceType);
            if (sourceConverter.CanConvertTo(destinationType))
                return sourceConverter.ConvertTo(null, culture, value, destinationType);

            if (destinationType.IsEnum && value is int)
                return Enum.ToObject(destinationType, (int)value);

            if (!destinationType.IsInstanceOfType(value))
                return Convert.ChangeType(value, destinationType, culture);

            return value;
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <returns>The converted value.</returns>
        public static T To<T>(object value)
        {
            //return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            return (T)To(value, typeof(T));
        }

        /// <summary>
        /// Convert enum for front-end
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Converted string</returns>
        public static string ConvertEnum(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            var result = string.Empty;
            foreach (var c in str)
                if (c.ToString() != c.ToString().ToLower())
                    result += " " + c.ToString();
                else
                    result += c.ToString();

            //ensure no spaces (e.g. when the first letter is upper case)
            result = result.TrimStart();
            return result;
        }

        /// <summary>
        /// Get difference in years
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int GetDifferenceInYears(DateTime startDate, DateTime endDate)
        {
            var age = endDate.Year - startDate.Year;
            if (startDate > endDate.AddYears(-age))
                age--;
            return age;
        }
    }
}