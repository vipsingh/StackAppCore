using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class DropdownWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.Dropdown; }
        public DropdownWidget(WidgetContext cntxt): base(cntxt)
        {
        }
    }
}
