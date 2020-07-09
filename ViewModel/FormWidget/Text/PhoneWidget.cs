using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class PhoneWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.Phone; }
        public PhoneWidget(WidgetContext cntxt): base(cntxt)
        {
        }
    }
}
