using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class LabelWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.Label; }
        public LabelWidget(WidgetContext cntxt): base(cntxt)
        {
            
        }
    }
}
