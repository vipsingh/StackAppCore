using System;
using System.Collections;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;

namespace StackErp.DB
{
    public class EntityQueryBuilder
    {
        IDBEntity _Entity;
        public EntityQueryBuilder(IDBEntity entity)
        {
            _Entity = entity;
        }

        public string BuildDetailQry()
        {
            var q = new DbQuery(_Entity);            

            bool hasItemTypeFiled = _Entity.DefaultItemTypeId > 0;
            List<string> ignoreFields = new List<string>();
            if (!hasItemTypeFiled) ignoreFields.Add("itemtype");

            var fields = _Entity.GetFields();
            foreach (var fk in fields.Keys)
            {
                var field = fields[fk];
                if (ignoreFields.Contains(field.Name.ToLower())) continue;

                if (field is OneToManyField) continue;

                q.AddField(field.Name, true);
            }

            q.ResolveFields();
            q.WhereInjectKeyword = "${WHERE}";

            var qBuilder = new DataList.QueryBuilder(q);
            var sql = qBuilder.BuildSql();

            return sql.Replace("${WHERE}", $" {_Entity.DBName}.id = ANY(@ItemId)");
        }
        
        public string PrepareRelatedFieldDataQueries(IEntityRelation relation, IDBEntity childEntity)
        {
            if (relation.Type == EntityRelationType.ManyToMany)
            {
                var relField = relation.ParentRefField.DBName;

                return $@"select {childEntity.IDField} as id, {childEntity.TextField} as name, pId from {childEntity.DBName} as c1 
                    join (
                        SELECT pField[s] as x, pId FROM (SELECT {relField} as pField, {_Entity.IDField} as pId, generate_subscripts({relField}, 1) AS s FROM {_Entity.DBName} where id = ANY(@ItemId) ) AS foo
                    ) as z 
                    on c1.{childEntity.IDField} = z.x;";  
            }

            return "";
        }
        
    }
}