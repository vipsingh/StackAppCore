using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class ListWidget: BaseWidget
    {
        public ListWidget(WidgetContext cntxt): base(cntxt)
        {
            
        }
        public override void OnCompile()
        {
            base.OnCompile();
            DataUrl = "/Entity/List?entity=UserMaster";
        }


    }
}
