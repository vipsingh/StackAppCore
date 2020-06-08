using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class TextWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.TextBox; }
        public TextWidget(WidgetContext cntxt): base(cntxt)
        {
        }
    }
}
