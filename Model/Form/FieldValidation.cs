using System;

namespace StackErp.Model
{
    public interface IFieldValidation
    {
        bool IsMendatory { set; get; }

        string ErrorMessage { set; get; }
        bool IsReadOnly { set; get; }
        short DecimalPlace { set; get; }

        double Min { set; get; }
        double Max { set; get; }
        string RegEx { set; get; }
    }

    public class FieldValidation : IFieldValidation
    {
        public bool IsMendatory { set; get; }
        public string ErrorMessage { set; get; }
        public bool IsReadOnly { set; get; }
        public short DecimalPlace { set; get; }
        public double Min { set; get; }
        public double Max { set; get; }
        public string RegEx { set; get; }
    }
}