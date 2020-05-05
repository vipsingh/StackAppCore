using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class EntityFilterWidget: ObjectPickerWidget
    {
        public override FormControlType WidgetType { get => FormControlType.EntityFilter; }
        public EntityFilterWidget(WidgetContext cntxt): base(cntxt)
        {
        }

        public override void OnCompile()
        {
            base.OnCompile();

        }
    }
}

