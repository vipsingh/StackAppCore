using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class NumberWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.NumberBox; }
        public NumberWidget(WidgetContext cntxt): base(cntxt)
        {
        }
    }
}
