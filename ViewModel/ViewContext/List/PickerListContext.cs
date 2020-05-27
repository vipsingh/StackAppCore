using System;
using System.Collections.Generic;
using System.Linq;
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
        public FieldDataSource DataSource {private set; get;}
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
            FieldDataSource dataSource = null;
            if (Field == null)
            {
               var reqInfo = ListRequest.RequestInfo;
               if (reqInfo.Properties != null)
               {
                   
               }
            } else {
                dataSource = Field.ControlInfo.DataSource;
            }

                if (dataSource != null)
                {
                    SourceEntityId = Field.ControlInfo.DataSource.Entity;
                }
                else
                    throw new AppException("Datasource is not defined for this list.");
            
            base.Init();
        } 

        protected override void PrepareFilter(DbQuery query, DataListDefinition defn) 
        {
            if (defn.FixedFilter == null) return;
            
            var formContext = new EditFormContext(this.Context, RelatedEntityId, new RequestQueryString() { });
            formContext.Build();

            var reqInfo = ListRequest.RequestInfo;
            formContext.PrepareLinkedData(reqInfo);
            
            var filters = defn.FixedFilter.DeepClone();
            ValueResolver.ResolveFilterExpressionValue(ref filters, formContext);

            query.SetFixedFilter(filters);

        }       
    }
}