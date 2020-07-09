using System;
using System.Collections;
using System.Collections.Generic;
using StackErp.Core.Layout;
using StackErp.DB.DataList;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;
using StackErp.Model.Layout;

namespace StackErp.Core.DataList
{
    public class EntityListService
    {
        public DataListDefinition GetEntityListDefn(EntityCode entityId, int queryId = 0)
        {
            var deff = ListDbService.GetEntityList(entityId);
            var _Entity = Core.EntityMetaData.Get(entityId);
            if (deff == null) 
            {
                deff = CreateDefaultListDefn(_Entity, entityId);
            }
            deff.Id = entityId.Code.ToString() + "_" + queryId.ToString();
            if (deff.PageSize <= 0)
                deff.PageSize = 50;

            return deff;
        }

        private EntityListDefinition CreateDefaultListDefn(IDBEntity entity, EntityCode entityId)
        {
            var defn = new EntityListDefinition()
            {
                EntityId = entityId.Code,
                Name = "Default",
                ItemIdField = entity.IDField,
                ItemViewField = entity.TextField,
                OrderByField  = new List<string>() { entity.TextField },
            };

            defn.DataSource = new FieldDataSource() 
            {
                Type = DataSourceType.Entity,
                Entity = entityId
            };

            List<TField> col_r = new List<TField>();
            var layoutF = entity.GetLayoutFields(EntityLayoutType.View);
            var tlist = new TList();
            foreach (var f in layoutF)
            {
                tlist.Fields.Add(new TField() { FieldId = f.Name });
            }
            defn.Layout = tlist;

            return defn;
        }

        public List<DynamicObj> ExecuteData(DbQuery query, Action<DynamicObj> onPrepareRow, Func<string, object, DynamicObj, object> onFormattedFieldValue)
        {
            var data = QueryDbService.ExecuteEntityQuery(query);
            return PrepareEntityData(data, query, onPrepareRow, onFormattedFieldValue);
        }

        public List<DynamicObj> PrepareEntityData(IEnumerable<DbObject> data, DbQuery query, Action<DynamicObj> onPrepareRow, Func<string, object, DynamicObj, object> onFormattedFieldValue)
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
                        row.Add(field.Field.Name, onFormattedFieldValue(field.Field.Name, field.Field.ResolveDbValue(dataRow), row), true);
                    } 
                    else 
                    {
                        row.Add(field.Field.Name, field.Field.ResolveDbValue(dataRow));
                    }
                }

                onPrepareRow(row);

                res.Add(row);
            }

            return res;
        }
    }
}
