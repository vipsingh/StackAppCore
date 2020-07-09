using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class PasswordWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.Password; }
        public PasswordWidget(WidgetContext cntxt): base(cntxt)
        {
        }
    }
}
