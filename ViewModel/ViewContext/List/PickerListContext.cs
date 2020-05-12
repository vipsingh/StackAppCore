using System;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.Model;

namespace StackErp.ViewModel.ViewContext
{
    public class PickerListContext: DataListContext
    {
        public BaseField Field {private set; get;}
        public EntityCode RelatedEntityId {private set; get;}
        public PickerListContext(StackAppContext context, ListRequestinfo requestInfo)
            : base(context, requestInfo)
        {

        }
        protected override void Init()
        {
            RelatedEntityId = RequestQuery.EntityId;    
            var RefEntity = Core.EntityMetaData.Get(RelatedEntityId);
                    
            Field = RefEntity.GetFieldSchema(RequestQuery.WidgetId);
            if (Field.ControlInfo.DataSource != null)
            {
                SourceEntityId = Field.ControlInfo.DataSource.Entity;
            }
            base.Init();
        }        
    }
}