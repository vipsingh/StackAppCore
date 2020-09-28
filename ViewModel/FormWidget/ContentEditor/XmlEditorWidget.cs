using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class XmlEditorWidget: LongTextWidget
    {
        public override FormControlType WidgetType { get => FormControlType.XmlEditor; }
        public XmlEditorWidget(WidgetContext cntxt): base(cntxt)
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
