using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class JsonEditorWidget: LongTextWidget
    {
        public override FormControlType WidgetType { get => FormControlType.JsonEditor; }
        public JsonEditorWidget(WidgetContext cntxt): base(cntxt)
        {
            
        }

        public override void OnCompile()
        {
            
        }

        private void FormatText()
        {
            
        }
    }
}
