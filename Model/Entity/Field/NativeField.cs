using System;

namespace StackErp.Model.Entity
{
    #region String Fields
    public class StringField: BaseField
    {
        public StringField(): base() {
            Type = FieldType.Text;
            BaseType = BaseTypeCode.String;
        }

        public override object ResolveSetValue(object val, out bool isValid)
        {
            isValid = true;
            if (val is string)
                return val;
            else
                return DataHelper.GetData(val, string.Empty);
        }
    }

    public class LongTextField: StringField
    {
        public LongTextField(): base() {
            Type = FieldType.LongText;
        }
    }

    public class EmailField: StringField
    {
        public EmailField(): base() {
            Type = FieldType.Email;
        }
    }

    public class JsonField: StringField
    {
        public JsonField(): base() {
            Type = FieldType.Json;
        }
    }

    public class XmlField: LongTextField
    {
        public XmlField(): base() {
            Type = FieldType.Xml;
        }
    }

    public class HtmlField: XmlField
    {
        public HtmlField(): base() {
            Type = FieldType.Html;
        }
    }

    #endregion

    #region Numeric Fields
    
    public abstract class NumericField: BaseField
    {
        public NumericField(): base() {

        }
    }
    public class IntegerField: NumericField
    {
        public IntegerField(): base() {
            Type = FieldType.Integer;
            BaseType = BaseTypeCode.Int32;
        }

        public override object ResolveSetValue(object val, out bool isValid)
        {
            isValid = true;

            var d = DataHelper.GetDataValue(val, TypeCode.Int32);

            return d;
        }

        public override object ResolveDbValue(DbObject db)
        {
            var v = db.Get(this.DBName, 0);

            return v;
        }
    }

    public class BigIntField: IntegerField
    {
        public BigIntField(): base() {
            Type = FieldType.BigInt;
            BaseType = BaseTypeCode.Int64;
        }
    }

    public class DecimalField: NumericField
    {
        public DecimalField(): base() {
            Type = FieldType.Decimal;
            BaseType = BaseTypeCode.Decimal;
        }

        public override object ResolveSetValue(object value, out bool isValid)
        {
            isValid = true;
            
            if (value is decimal)
                    {
                        return value;
                    }
                    else
                    {
                        decimal doubleValue;
                        if(decimal.TryParse(value.ToString(), out doubleValue))
                            return doubleValue;
                        else
                        {
                            isValid= false;
                            return value;
                        }
                    }
        }

        public override object ResolveDbValue(DbObject db)
        {
            var v = db.Get<object>(this.DBName, null);

            if(v == null)
                return null;
            else
                return (decimal)v;
        }
    }

    public class MonataryField: DecimalField
    {
        public string CurrencyField {set;get;}
        public MonataryField(): base() {
            Type = FieldType.MonataryAmount;
        }
    }

    #endregion

    public class ObjectKeyField: IntegerField
    {
        public ObjectKeyField(): base() {
            Type = FieldType.ObjectKey;
            Copy = false;
            IsReadOnly = true;
        }
    }
    public class BoolField: BaseField
    {
        public BoolField(): base() {
            Type = FieldType.Bool;
            BaseType = BaseTypeCode.Boolean;            
        }

        public override object ResolveSetValue(object val, out bool isValid)
        {
            isValid = true;
            
            var d = DataHelper.GetDataValue(val, TypeCode.Boolean);

            return d;
        }
        public override object ResolveDbValue(DbObject db)
        {
            var v = db.Get<object>(this.DBName, null);

            if(v == null)
                return false;
            else
                return DataHelper.GetDataValue(v, TypeCode.Boolean);
        }
    }

    public class DateTimeField: BaseField
    {
        public DateTimeField(): base() {
            Type = FieldType.DateTime;
            BaseType = BaseTypeCode.DateTime;
        }

        public override object ResolveDbValue(DbObject db)
        {
            var v = db.Get<string>(this.DBName, null);

            if(v == null || v == "")
                return DateTime.MinValue;
            else
                return DateTime.Parse(v);
        }
    }

    public class DateField: DateTimeField
    {
        public DateField(): base() {
            Type = FieldType.Date;
        }
    }

    public class TimeField: DateTimeField
    {
        public TimeField(): base() {
            Type = FieldType.Time;
        }
    }

    public class ImageField: JsonField
    {
        public ImageField(): base() {
            Type = FieldType.Image;
            BaseType = BaseTypeCode.String;
        }
    }
}