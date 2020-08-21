using System;
using System.Collections.Generic;
using System.Linq;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;
using StackErp.Model.Form;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.Model;

namespace StackErp.ViewModel.ViewContext
{
    public class DataListContext
    {
        public StackAppContext Context { get; }
        public ListRequestinfo ListRequest { private set; get;}
        private DynamicObj _parms;
        public DynamicObj Parameters { get => _parms; }
        private DynamicObj _props;
        public DynamicObj Properties { get => _props; }
        protected RequestQueryString RequestQuery {get;}
        protected DataListRequestType _RequestMode;
        public EntityCode SourceEntityId {protected set; get;}
        public IDBEntity SourceEntity {protected set; get;}
        public DbQuery DbQuery {protected set; get;}
        public InvariantDictionary<IDisplayWidget> Fields  {set; get;} 
        public List<ActionLinkDefinition> RowLinks {set; get;}
        protected int _DefaultPageSize;
        public List<DynamicObj> Data {set; get;} 
        public  string IdColumn {set; get;} 
        public string ItemViewField {set; get;} 

        public DataListContext(StackAppContext context, RequestQueryString query, ListRequestinfo requestInfo)
        {
            Context = context;
            RequestQuery = query;
            ListRequest = requestInfo;
            _parms = new DynamicObj();
            _props = new DynamicObj();
            Fields = new InvariantDictionary<IDisplayWidget>();
            RowLinks = new List<ActionLinkDefinition>();
             _RequestMode = ListRequest.RequestType;
             SourceEntityId = RequestQuery.EntityId;
            Init();
        }

        protected virtual void Init()
        {                       
            SourceEntity = Core.EntityMetaData.Get(SourceEntityId);
        }

        public T GetModelInfo<T>(string key, T defaultVal)
        {
            var requset = ListRequest.RequestInfo;
            if (requset == null || requset.EntityInfo == null)
                return defaultVal;

            return requset.EntityInfo.Get(key, defaultVal);
        }

        public virtual void BuildSource(DataListDefinition defn)
        {
            var entity = Core.EntityMetaData.Get(defn.DataSource.Entity);
            this.DbQuery = new DbQuery(entity);
            this.DbQuery.BuildListDefn(defn);
            this.IdColumn = this.DbQuery.ItemIdField;
            this.ItemViewField = defn.ItemViewField;
            
            PrepareFilter(this.DbQuery, defn);
            PrepareRequestFilter(this.DbQuery, ListRequest.GridRequest);            

            var gridReq = ListRequest.GridRequest;
            if (gridReq != null)
            {
                DbQuery.WithPage(ListRequest.GridRequest.PageIndex, ListRequest.GridRequest.PageSize);
            } 
            else
                DbQuery.WithPage(0, defn.PageSize);
        }

        protected virtual void PrepareFilter(DbQuery query, DataListDefinition defn) 
        {           
        }

        protected virtual void PrepareRequestFilter(DbQuery query, QueryRequest gridReq)
        {
            if (gridReq == null || gridReq.DataFilter == null) return;

            var filter = FilterExpression.BuildFromJson(SourceEntityId, gridReq.DataFilter.ToString());

            query.SetFilter(filter);
        }
    }
}
