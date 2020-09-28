using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class DatePickerWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.DatePicker; }
        public DatePickerWidget(WidgetContext cntxt): base(cntxt)
        {
            //EntityDataFormatter
        }

        protected override bool OnSetData(object value)
        {
            if (value != null && value is string) {
                value = DataHelper.GetData<DateTime>(value, DateTime.MinValue);
            }
            if (value is DateTime)
            {
                var v =(DateTime)value;
                if(v == DateTime.MinValue)
                    this.Value = null;
                else
                    this.Value = v;

                return true;
            }
            
            return false;
        }
    }
}
