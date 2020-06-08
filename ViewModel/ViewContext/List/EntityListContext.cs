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
    public class EntityListContext: DataListContext
    {
        public EntityListContext(StackAppContext context, RequestQueryString query, ListRequestinfo requestInfo)
            : base(context, query, requestInfo)
        {

        }
    }
    public class RelatedEntityListContext: DataListContext
    {
        public BaseField EntityField {private set; get;}
        public FieldDataSource DataSource {private set; get;}
        public EntityCode RelatedEntityId {private set; get;}
        public RelatedEntityListContext(StackAppContext context, RequestQueryString query, ListRequestinfo requestInfo)
            : base(context, query, requestInfo)
        {

        }
        protected override void Init()
        {
            RelatedEntityId = RequestQuery.EntityId;    
            var RefEntity = Core.EntityMetaData.Get(RelatedEntityId);
                    
            EntityField = RefEntity.GetFieldSchema(RequestQuery.FieldName);
            FieldDataSource dataSource = null;
            // if (Field == null)
            // {
            //    var reqInfo = ListRequest.RequestInfo;
            //    if (reqInfo.Properties != null)
            //    {
                   
            //    }
            // } else {
            //     dataSource = Field.ControlInfo.DataSource;
            // }

            //     if (dataSource != null)
            //     {
            //         SourceEntityId = Field.ControlInfo.DataSource.Entity;
            //     }
            //     else
            //         throw new AppException("Datasource is not defined for this list.");
            
            base.Init();
        } 

        protected override void PrepareFilter(DbQuery query, DataListDefinition defn) 
        {
            

        }       
    }
}