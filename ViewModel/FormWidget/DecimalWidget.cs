using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class DecimalWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.DecimalBox; }
        public DecimalWidget(WidgetContext cntxt): base(cntxt)
        {
        }
    }
}
