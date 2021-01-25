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

        private EntityCode _entityId;
        private string relatedField;

        public EntityListWidget(WidgetContext cntxt): base(cntxt)
        {
            _entityId = this.Context.FormContext.EntityId;
        }

        public EntityListWidget(WidgetContext cntxt, EntityCode entityId): base(cntxt)
        {
            _entityId = entityId;
        }

        public EntityListWidget(WidgetContext cntxt, string RelationField): this(cntxt)
        {
            relatedField  = RelationField;
        }
        public override void OnCompile()
        {
            PrepareFilterBox();
            base.OnCompile();        
        }

        protected override bool OnFormatSetData(object value)
        {            
            DataActionLink = new ActionInfo("/Entity/List", GetReqQuery(), "DETAIL");

            return base.OnSetData(value);
        }

        private RequestQueryString GetReqQuery()
        {
            var rQ = new RequestQueryString();
            rQ.EntityId = _entityId;
            
            if(!string.IsNullOrEmpty(relatedField))
            {
                var entityModel = this.Context.FormContext.EntityModelInfo;
                rQ.ItemId = entityModel.ObjectId;
                rQ.RelationField = this.relatedField;
            }

            return rQ;
        }

        private void PrepareFilterBox()
        {
            var rQ = new RequestQueryString();
            rQ.EntityId = _entityId;

            var editFormContext = new EditFormContext(this.Context.FormContext.Context, _entityId, rQ);
            editFormContext.Build();

            var cntxt = new WidgetContext(editFormContext);
            cntxt.Build(null, new StackErp.Model.Layout.TField(){ FieldId = "FilterBox", WidgetType = FormControlType.EntityFilter });
            cntxt.ControlDefinition = new StackErp.Model.Entity.ControlDefinition()
            {
                DataSource = new StackErp.Model.Entity.FieldDataSource
                {
                    Type = DataSourceType.Entity,
                    Entity = EntityCode.EntitySchema,                
                }
            };

            var widget = new EntityFilterWidget(cntxt);
            widget.FilterEntityId = _entityId;
            widget.OnCompile();

            FilterBox = widget;
        }

    }
}
