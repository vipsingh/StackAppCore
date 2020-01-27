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
        RequestQueryString RequestQuery {get => Context.RequestQuery;}
        private DataListRequestType _RequestMode;
        public EntityCode SourceEntityId {protected set; get;}
        public IDBEntity SourceEntity {protected set; get;}
        public DbQuery DbQuery {protected set; get;}
        public Dictionary<string, BaseWidget> Fields  {set; get;} 

        public DataListContext(StackAppContext context, ListRequestinfo requestInfo)
        {
            Context = context;
            ListRequest = requestInfo;
            _parms = new DynamicObj();
            _props = new DynamicObj();
            Fields = new Dictionary<string, BaseWidget>();
            Init();
        }

        private void Init()
        {
            _RequestMode = ListRequest.RequestType;
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

        public void BuildSource(DataListDefinition defn)
        {
            this.DbQuery = new DbQuery(this.SourceEntity);
            this.DbQuery.BuildWithLayout(defn.Layout);
            this.DbQuery.ResolveFields();
        }
    }
}
