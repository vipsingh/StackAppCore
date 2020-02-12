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
            defn.EntityId = entityId;
            defn.ItemIdField = "Id";
            defn.Id = entityId.Code.ToString() + "_" + queryId.ToString();
            defn.Layout = layout.PrepareListLayout(queryId);
            defn.PageSize = 50; 

            return defn;
        }

        public List<DynamicObj> ExecuteData(DbQuery query)
        {
            var data = QueryDbService.ExecuteEntityQuery(query);
            return PrepareEntityData(data, query);
        }

        public List<DynamicObj> PrepareEntityData(IEnumerable<DbObject> data, DbQuery query)
        {
            var res = new List<DynamicObj>();
            foreach(var dataRow in data)
            {
                var row = new DynamicObj();
                foreach(var field in query.Fields)
                {
                    if (field.IsSelect)
                    {
                        row.Add(field.Field.ViewName, field.Field.ResolveDbValue(dataRow), true);
                    }
                }
                res.Add(row);
            }

            return res;
        }
    }
}
