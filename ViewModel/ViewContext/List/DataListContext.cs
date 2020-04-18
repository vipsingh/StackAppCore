using System;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;
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
        protected RequestQueryString RequestQuery {get => Context.RequestQuery;}
        protected DataListRequestType _RequestMode;
        public EntityCode SourceEntityId {protected set; get;}
        public IDBEntity SourceEntity {protected set; get;}
        public DbQuery DbQuery {protected set; get;}
        public InvariantDictionary<BaseWidget> Fields  {set; get;} 
        protected int _DefaultPageSize;
        public List<DynamicObj> Data {set; get;} 
        public  string IdColumn {set; get;} 
        public string ItemViewField {set; get;} 

        public DataListContext(StackAppContext context, ListRequestinfo requestInfo)
        {
            Context = context;
            ListRequest = requestInfo;
            _parms = new DynamicObj();
            _props = new DynamicObj();
            Fields = new InvariantDictionary<BaseWidget>();
             _RequestMode = ListRequest.RequestType;
            Init();
        }

        protected virtual void Init()
        {           
            SourceEntityId = RequestQuery.EntityId;
            SourceEntity = Core.EntityMetaData.Get(SourceEntityId);
        }

        public T GetModelInfo<T>(string key, T defaultVal)
        {
            var requset = ListRequest.RequestInfo;
            if (requset == null || requset.ModelInfo == null)
                return defaultVal;

            return requset.ModelInfo.Get(key, defaultVal);
        }

        public virtual void BuildSource(DataListDefinition defn)
        {
            var entity = Core.EntityMetaData.Get(defn.EntityId);
            this.DbQuery = new DbQuery(entity);
            this.DbQuery.ItemIdField = defn.ItemIdField;
            
            if (defn.Layout != null)
            {
                this.DbQuery.BuildWithLayout(defn.Layout);
            }
            this.DbQuery.ResolveFields();
            this.IdColumn = defn.ItemIdField;
            this.ItemViewField = defn.ItemViewField;

            var gridReq = ListRequest.GridRequest;
            if (gridReq != null)
            {
                DbQuery.WithPage(ListRequest.GridRequest.PageIndex, ListRequest.GridRequest.PageSize);
            } 
            else
                DbQuery.WithPage(0, defn.PageSize);
        }
    }
}
