using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class HtmlTextWidget: LongTextWidget
    {
        public override FormControlType WidgetType { get => FormControlType.HtmlText; }
        public HtmlTextWidget(WidgetContext cntxt): base(cntxt)
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
