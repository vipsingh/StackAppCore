using System;

namespace StackErp.ViewModel.Model
{
    public interface IValidation
    {
        string Msg { get; set; }
    }

    public class RequiredValidation : IValidation
    {
        public string Msg { get; set; }
    }

     public class RegExValidation : IValidation
    {
        public string Msg { get; set; }
        public string Expression { get; set; }
    }

    public class RangeValidation : IValidation
    {
        public string Msg { get; set; }
        public object Min { get; set; }
        public object Max { get; set; }

        public RangeValidation(object min, object max)
        {
            Min = min;
            Max = max;
        }
    }
    public class LengthValidation : IValidation
    {
        public string Msg { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }

        public LengthValidation()
        {
        }

        public LengthValidation(int min, int max)
        {
            Min = min;
            Max = Max;
        }
    }
}
