using System;
using StackErp.Model;
using StackErp.Model.Form;
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
            DataActionLink = new ActionInfo("/Entity/List", this.Context.FormContext.RequestQuery, "DETAIL");
        }


    }
}
