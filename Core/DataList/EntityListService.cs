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

        public IEnumerable<DbObject> ExecuteData(DbQuery query)
        {
            var data = QueryDbService.ExecuteEntityQuery(query);
            return data;
        }
    }
}
