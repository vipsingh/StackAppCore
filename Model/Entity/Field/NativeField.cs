using System;

namespace StackErp.Model.Entity
{
    #region String Fields
    public class StringField: BaseField
    {
        public StringField(): base() {
            Type = FieldType.Text;
            BaseType = TypeCode.String;
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


    public class PasswordField: StringField
    {
        public PasswordField(): base() {
            Type = FieldType.Password;
        }
    }

    public class EmailField: StringField
    {
        public EmailField(): base() {
            Type = FieldType.Email;
        }
    }

    public class PhoneField: StringField
    {
        public PhoneField(): base() {
            Type = FieldType.Phone;
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
            BaseType = TypeCode.Int32;
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
            BaseType = TypeCode.Int64;
        }
    }

    public class DecimalField: NumericField
    {
        public DecimalField(): base() {
            Type = FieldType.Decimal;
            BaseType = TypeCode.Decimal;
        }

        public override void OnInit()
        {
            base.OnInit();

            FormatInfo.FormatString = "{0:0.00}";
        }

        public override object ResolveSetValue(object value, out bool isValid)
        {
            isValid = true;

            if (value == null) return value;
            
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
            BaseType = TypeCode.Boolean;            
        }

        public override object ResolveSetValue(object val, out bool isValid)
        {
            isValid = true;
            
            var d = DataHelper.GetDataValue(val, BaseType);

            return d;
        }
        public override object ResolveDbValue(DbObject db)
        {
            var v = db.Get<object>(this.DBName, null);

            if(v == null)
                return false;
            else
                return DataHelper.GetDataValue(v, BaseType);
        }
    }

    public class DateTimeField: BaseField
    {
        public DateTimeField(): base() {
            Type = FieldType.DateTime;
            BaseType = TypeCode.DateTime;
        }

        public override void OnInit()
        {
            base.OnInit();

            FormatInfo.FormatString = "dt";
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
        public bool IgnoreTimeZone {set;get;}
        public DateField(): base() {
            Type = FieldType.Date;
        }

        public override void OnInit()
        {
            base.OnInit();

            FormatInfo.FormatString = "d";
        }
    }

    public class TimeField: DateTimeField
    {
        public TimeField(): base() {
            Type = FieldType.Time;
        }
    }
    
}