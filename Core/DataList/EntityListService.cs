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
            var _Entity = Core.EntityMetaData.Get(entityId);
            var defn = new DataListDefinition();
            defn.EntityId = entityId;
            defn.ItemIdField = "Id";
            defn.Id = entityId.Code.ToString() + "_" + queryId.ToString();
            defn.Layout = layout.PrepareListLayout(queryId);
            defn.ItemViewField = _Entity.GetFieldSchema(_Entity.TextField).ViewName;
            defn.PageSize = 50; 

            return defn;
        }

        public List<DynamicObj> ExecuteData(DbQuery query, Func<string, object, DynamicObj, object> onFormattedFieldValue)
        {
            var data = QueryDbService.ExecuteEntityQuery(query);
            return PrepareEntityData(data, query, onFormattedFieldValue);
        }

        public List<DynamicObj> PrepareEntityData(IEnumerable<DbObject> data, DbQuery query, Func<string, object, DynamicObj, object> onFormattedFieldValue)
        {
            var res = new List<DynamicObj>();
            var idField = query.Entity.GetFieldSchema(query.ItemIdField);
            foreach(var dataRow in data)
            {
                var row = new DynamicObj();
                row.Add("RowId", dataRow.Get(idField.ViewName, 0));

                foreach(var field in query.Fields)
                {
                    if (field.IsSelect)
                    {
                        row.Add(field.Field.ViewName, onFormattedFieldValue(field.Field.ViewName, field.Field.ResolveDbValue(dataRow), row), true);
                    }
                }                                

                res.Add(row);
            }

            return res;
        }
    }
}
