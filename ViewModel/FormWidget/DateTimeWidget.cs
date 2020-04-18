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

        protected override bool OnFormatSetData(object value)
        {
            var formatter = new Core.Entity.EntityDataFormatter(this.Context.AppContext);
            var val =  formatter.FormatDate(value);

            return base.OnFormatSetData(val);
        }
    }
}
