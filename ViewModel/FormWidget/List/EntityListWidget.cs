using System;
using StackErp.Model;
using StackErp.Model.Form;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class EntityListWidget: ListWidget
    {
        public override FormControlType WidgetType { get => FormControlType.EntityListView; }
        public EntityFilterWidget FilterBox {set;get;}

        public EntityListWidget(WidgetContext cntxt): base(cntxt)
        {

        }
        public override void OnCompile()
        {
            PrepareFilterBox();
            base.OnCompile();
            DataActionLink = new ActionInfo("/Entity/List", this.Context.FormContext.RequestQuery, "DETAIL");
        }

        private void PrepareFilterBox()
        {
            var editFormContext = new EditFormContext(this.Context.FormContext.Context, this.Context.FormContext.RequestQuery.EntityId, this.Context.FormContext.RequestQuery);
            editFormContext.Build();

            var cntxt = new WidgetContext(editFormContext);
            cntxt.Build(null, new StackErp.Model.Layout.TField(){ FieldId = "FilterBox", Widget = FormControlType.EntityFilter });
            cntxt.ControlDefinition = new StackErp.Model.Entity.ControlDefinition()
            {
                DataSource = new StackErp.Model.Entity.FieldDataSource
                {
                    Type = DataSourceType.Entity,
                    Entity = EntityCode.EntitySchema,                
                }
            };

            var widget = new EntityFilterWidget(cntxt);
            widget.FilterEntityId = this.Context.FormContext.RequestQuery.EntityId;
            widget.OnCompile();

            FilterBox = widget;
        }

    }
}
