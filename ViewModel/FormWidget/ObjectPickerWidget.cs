using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class ObjectPickerWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.EntityPicker; }
        public ObjectPickerWidget(WidgetContext cntxt): base(cntxt)
        {
        }
    }
}
