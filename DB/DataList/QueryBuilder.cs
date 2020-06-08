
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;

namespace StackErp.DB.DataList
{
    public class QueryDbService
    {
        public static IEnumerable<DbObject> ExecuteEntityQuery(DbQuery query)
        {
            query.ResolveFields();
            
            var builder = new QueryBuilder(query);
            var data = DBService.Query(builder.BuildSql(), new {});
            return data;
        }
    }
    public class QueryBuilder
    {
        DbQuery _QueryInfo;
        private bool IsDynamic {get => !_QueryInfo.IsFixedQuery;}
        public QueryBuilder(DbQuery queryInfo)
        {
            _QueryInfo = queryInfo;
        }
        List<(string, string, string)> _selectFields;
        List<(string, string, string)> _toJoinTables;
        string _entTable;
        short idx = 0;
        public string BuildSql()
        {
            _entTable = _QueryInfo.Entity.DBName;

            _selectFields = new List<(string, string, string)>();
            _toJoinTables = new List<(string, string, string)>();

            AddSelectField("ID", _entTable + ".id", "id");

            var fields = _QueryInfo.Fields;
            
            var relations = _QueryInfo.Relations;

            foreach (var field in fields)
            {
                var fieldSchema = field.Field;
                if (field.IsSelect)
                {
                    AddSelectField(field.FieldName, _entTable + "." + fieldSchema.DBName, field.FieldName);

                    if (fieldSchema.RefObject.Code > 0)
                    {
                        var rel = relations.Find(x => x.ParentRefField.Name == fieldSchema.Name);
                        if(rel != null)
                        {
                            AddRefTable(rel);
                        }
                    }
                }
            }

            var selectSql = PrepareSelectQuery();
            var where = PrepareWhereExp(_QueryInfo, _QueryInfo.FixedFilter, _QueryInfo.Filters);

            var q = selectSql + (String.IsNullOrEmpty(where)? "" : " WHERE " + where);

            return q;
        }

        void AddRefTable(IEntityRelation rel)
        {
            var idf = "_" + idx;
            var refEnt = _QueryInfo.Entity.GetEntity(rel.ChildName);
            var shouldJoin = false;
                if (rel.Type == EntityRelationType.LINK)
                {
                    AddSelectField(rel.ParentRefField.Name, $"{idf}.{rel.ChildDisplayField.DBName}", $"{rel.ParentRefField.Name}__name");                    
                    //_addedFields.Add((rel.ParentRefField.Name, rel.ParentRefField.DBName, rel.ParentRefField.Name));
                    shouldJoin = true;
                }

                if (shouldJoin)
                {
                    _toJoinTables.Add(($"{refEnt.DBName} as {idf}", $"{_entTable}.{rel.ParentRefField.DBName}", $"{idf}.id"));
                }

            idx++;
        }

        private void AddSelectField(string fieldName, string queryName, string queryAlias)
        {
            if (_selectFields.Where(x => x.Item2.ToLower() == queryName.ToLower()).Count() == 0)
                _selectFields.Add((fieldName, queryName, queryAlias));
        }

        string PrepareSelectQuery()
        {
            List<string> selectExp = new List<string>();
            _selectFields.ForEach(x =>
            {
                selectExp.Add($"{x.Item2} as {x.Item3}");
            });

            string qry = String.Format("SELECT {0} FROM {1}", String.Join(",", selectExp), _entTable);

            if (_toJoinTables != null)
            {
                _toJoinTables.ForEach(x =>
                {
                    qry += $" LEFT JOIN {x.Item1} ON {x.Item2} = {x.Item3}";
                });
            }            

            return qry;
        }

        private List<DynamicDbParam> PrepareParameters()
        {
            var param = new List<DynamicDbParam>();            

            return param;
        }

        private List<DynamicDbParam> GetFixedParameters()
        {
            var param = new List<DynamicDbParam>();            

            return param;
        }

        public static string PrepareWhereExp(DbQuery qInfo, FilterExpression fixedFilter, FilterExpression filters)
        {
            var whereFix = PrepareFilterSql(qInfo, fixedFilter);

            var whereQ = PrepareFilterSql(qInfo, filters);

            if (string.IsNullOrEmpty(whereFix))
                return whereQ;

            return whereFix + (String.IsNullOrEmpty(whereQ) ? "": $" AND ({whereQ})"); 
        }
        public static string PrepareFilterSql(DbQuery qInfo, FilterExpression filterExp)
        {
            var sql = new List<string>();
            if (filterExp != null)
            {
                foreach(var filter in filterExp.GetAll())
                {
                    var dbField = qInfo.Fields.Find(x=>x.FieldName == filter.FieldName);                   
                    sql.Add(getFieldFilterString(dbField, filter));
                }
            }
            return String.Join(" AND ", sql);
        }

        private static string getFieldFilterString(DbQueryField dbField, FilterExpField filter)
        {
            var op = GetSqlOpOp(filter.Op, filter.Value == null ? "" : filter.Value.ToString());

            string val;
            if (filter.Op == FilterOperationType.In || filter.Op == FilterOperationType.NotIn)
            {
                List<string> v1 = new List<string>();
                if (filter.Value != null)
                {
                    String[] vals = filter.Value.ToString().Split(',');
                    foreach(var d in vals)
                    {
                        v1.Add(GetSqlVal(dbField, d));
                    }
                    val = "(" + string.Join(",", v1) + ")";
                }
                else
                {
                    val = "(" + GetSqlVal(dbField, op.Item2) + ")";    
                }
                
            } 
            else 
            {
                val = GetSqlVal(dbField, op.Item2);
            }

            var s = String.Format("{0}.{1} {2} {3}", dbField.Alias, dbField.DbName, op.Item1, val);
            
            return s;
        }

        private static string GetSqlVal(DbQueryField dbQueryField, string v)
        {
                string v1 = v;
                if (IsNumericVal(dbQueryField.Field))
                {
                    if (string.IsNullOrEmpty(v)) {
                        v1 = "-1";
                    }
                }
                else {
                    v1 = "'" + v + "'";
                }

                return v1;
        }
        private static bool IsNumericVal(BaseField f)
        {
            if(f.BaseType == TypeCode.Int32 || f.BaseType == TypeCode.Int16 || f.BaseType == TypeCode.Int64 || f.BaseType == TypeCode.Decimal)
            {
                return true;
            }
            return false;
        }

        internal static (string, string) GetSqlOpOp(FilterOperationType filterOp, string value)
        {
            var op = "=";
            var val = value;
            var likeKW = "ilike"; //for postgre case insenstive search
            if (filterOp == FilterOperationType.NotEqual)
                op = "!=";
            else if (filterOp == FilterOperationType.GreaterThan)
                op = ">";
            else if (filterOp == FilterOperationType.GreaterThanEqual)
                op = ">=";
            else if (filterOp == FilterOperationType.LessThan)
                op = "<";
            else if (filterOp == FilterOperationType.LessThanEqual)
                op = "<=";
            else if (filterOp == FilterOperationType.Like)
                op = likeKW;
            else if (filterOp == FilterOperationType.Contains)
            {
                op = likeKW; val = "%" + value + "%";
            }
            else if (filterOp == FilterOperationType.StartWith)
            {
                op = likeKW; val = "" + value + "%";
            }
            else if (filterOp == FilterOperationType.EndWith)
            {
                op = likeKW; val = "%" + value + "";
            }
            else if (filterOp == FilterOperationType.In)
            {
                op = "IN";
            }
            else if (filterOp == FilterOperationType.NotIn)
            {
                op = "NOT IN";
            }
            //between
            return (op, val);
        }
    }
}