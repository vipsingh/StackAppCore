using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class CheckBoxWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.CheckBox; }
        public CheckBoxWidget(WidgetContext cntxt): base(cntxt)
        {
        }
    }
}

