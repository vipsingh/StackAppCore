using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class StackScriptWidget: LongTextWidget
    {
        public override FormControlType WidgetType { get => FormControlType.StackScriptEditor; }
        public StackScriptWidget(WidgetContext cntxt): base(cntxt)
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
