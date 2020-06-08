using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class LongTextWidget: TextWidget
    {
        public override FormControlType WidgetType { get => FormControlType.LongText; }
        public LongTextWidget(WidgetContext cntxt): base(cntxt)
        {
            
        }

        public override void OnCompile()
        {
            //this.AddAttribute("Rows", 3);
            //this.AddAttribute("ShowTextLength", false);
        }
    }
}
