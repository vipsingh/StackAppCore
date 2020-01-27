using System;
using System.Collections;
using System.Collections.Generic;
using StackErp.Core.Layout;
using StackErp.DB.DataList;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;

namespace StackErp.Core.DataList
{
    public class EntityListService
    {
        public DataListDefinition GetEntityListDefn(EntityCode entityId, int queryId = 0)
        {
            var layout = new EntityLayoutService(null, entityId);
            var defn = new DataListDefinition();
            defn.Id = entityId.Code.ToString() + "_" + queryId.ToString();
            defn.Layout = layout.PrepareListLayout(queryId);
            defn.Query = null;

            return defn;
        }

        public IEnumerable<DbObject> ExecuteData(DbQuery query)
        {
            var data = QueryDbService.ExecuteEntityQuery(query);
            return data;
        }
    }
}
