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
        }
    }
}
