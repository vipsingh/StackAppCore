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
        public PickerListContext(StackAppContext context , RequestQueryString query, ListRequestinfo requestInfo)
            : base(context, query, requestInfo)
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
                   var source = reqInfo.Properties.Get("PickerSource", "");
                   dataSource = Core.Datasource.SystemDataSource.GetPickerSource(source);
               }
            } 
            else 
            {
                dataSource = Field.ControlInfo.DataSource;
            }

            if (dataSource != null)
            {
                DataSource = dataSource;
                SourceEntityId = dataSource.Entity;
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
            
            if (reqInfo.Properties != null)
            {
                var sourceParams = reqInfo.Properties.Get<DynamicObj>("PickerSourceParams", null);
                if(sourceParams != null)
                {
                    formContext.AddEntityModelInfo("Parameters", sourceParams);
                }
            }
            
            var filters = defn.FixedFilter.DeepClone();
            ValueResolver.ResolveFilterExpressionValue(ref filters, formContext);

            query.SetFixedFilter(filters);

        }       
    }
}