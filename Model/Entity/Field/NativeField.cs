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
        }
    }
    public class BoolField: BaseField
    {
        public BoolField(): base() {
            Type = FieldType.Bool;
            BaseType = BaseTypeCode.Boolean;
        }
    }

    public class DateTimeField: BaseField
    {
        public DateTimeField(): base() {
            Type = FieldType.DateTime;
            BaseType = BaseTypeCode.DateTime;
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