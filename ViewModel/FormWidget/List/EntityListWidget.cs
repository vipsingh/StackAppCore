using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class EntityListWidget: ListWidget
    {
        public override FormControlType WidgetType { get => FormControlType.EntityListView; }

        public EntityListWidget(WidgetContext cntxt): base(cntxt)
        {
            
        }
        public override void OnCompile()
        {
            base.OnCompile();
            DataUrl = "/Entity/List?entity=UserMaster";
        }


    }
}
