using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class EmailWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.Email; }
        public EmailWidget(WidgetContext cntxt): base(cntxt)
        {
        }
    }
}
