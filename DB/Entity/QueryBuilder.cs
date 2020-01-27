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
            string entTable = _Entity.DBName;

            var addedFields = new List<(string, string, string)>();
            var toJoinTables = new List<(string, string, string)>();
            addedFields.Add(("ID", entTable + ".id", "id"));

            var fields = _Entity.GetFields();
            short idx = 0;
            foreach (var rel in _Entity.Relations)
            {
                var idf = "_" + idx;
                var refEnt = _Entity.GetEntity(rel.ChildName);

                var shouldJoin = false;
                if (rel.Type == EntityRelationType.LINK)
                {
                    addedFields.Add((rel.ParentRefField.Name, $"{idf}.{rel.ChildRefField.DBName}", $"{rel.ParentRefField.Name}__name"));
                    addedFields.Add((rel.ParentRefField.Name, rel.ParentRefField.DBName, rel.ParentRefField.Name));
                    shouldJoin = true;
                }

                if (shouldJoin)
                {
                    toJoinTables.Add(($"{refEnt.DBName} as {idf}", $"{entTable}.{rel.ParentRefField.DBName}", $"{idf}.id"));
                }
                idx++;
            }

            foreach (var fk in fields.Keys)
            {
                var field = fields[fk];
                if (addedFields.FindAll(x => x.Item1.ToUpper() == field.Name.ToUpper()).Count == 0)
                    addedFields.Add((field.Name, entTable + "." + field.DBName, field.Name));
            }

            return PrepareSelectQuery(entTable, addedFields, toJoinTables, $" {entTable}.id = @ItemId");
        }

        string PrepareSelectQuery(string table, List<(string, string, string)> select, List<(string, string, string)> joinTables, string where)
        {
            List<string> selectExp = new List<string>();
            select.ForEach(x =>
            {
                selectExp.Add($"{x.Item2} as {x.Item3}");
            });

            string qry = String.Format("SELECT {0} FROM {1}", String.Join(",", selectExp), table);

            if (joinTables != null)
            {
                joinTables.ForEach(x =>
                {
                    qry += $" LEFT JOIN {x.Item1} ON {x.Item2} = {x.Item3}";
                });
            }
            qry += " WHERE " + where;

            return qry;
        }
        
    }
}