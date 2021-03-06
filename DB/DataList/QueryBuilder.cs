
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
        public static IEnumerable<DbObject> ExecuteEntityQuery(StackAppContext appContext, DbQuery query)
        {
            query.ResolveFields();
            
            var builder = new QueryBuilder(query);
            var data = DBService.Query(builder.BuildSql(), new { MasterId = appContext.MasterId });

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
        Dictionary<string, (string, string, List<string>)> _toJoinTables;
        string _entTable;
        short idx = 0;
        public string BuildSql()
        {
            _entTable = _QueryInfo.Entity.DBName;

            _selectFields = new List<(string, string, string)>();
            _toJoinTables = new Dictionary<string, (string,string, List<string>)>();

            AddSelectField("ID", _entTable + ".id", "id");

            var fields = _QueryInfo.Fields;
            
            var relations = _QueryInfo.Relations;

            foreach (var field in fields)
            {
                var fieldSchema = field.Field;
                if (field.IsSelect)
                {
                    if (!string.IsNullOrEmpty(field.DbName))
                        AddSelectField(field.FieldName, _entTable + "." + fieldSchema.DBName, field.FieldName);

                    if (fieldSchema.RefObject.Code > 0)
                    {
                        var rel = relations.Find(x => x.ParentRefField.Name == fieldSchema.Name);
                        if (fieldSchema.IsRelatedField) 
                        {
                            rel = relations.Find(x => x.ParentRefField.Name == fieldSchema.Related.LinkField);
                        }
                        if(rel != null)
                        {
                            AddRefTable(rel);
                        }
                    }

                    if (fieldSchema.IsRelatedField)
                    {
                        var tab = _toJoinTables[fieldSchema.Related.LinkField];
                        var refField = fieldSchema.Related.Field;
                        AddSelectField(field.FieldName, $"{tab.Item2}.{refField}", field.FieldName); 
                    }

                    if (fieldSchema is SelectField)
                    {
                        AddCollectionTable(fieldSchema);
                    }
                }
            }

            var selectSql = PrepareSelectQuery();
            var where = PrepareWhereExp(_QueryInfo, _entTable, _QueryInfo.FixedFilter, _QueryInfo.Filters);

            if (!String.IsNullOrEmpty(_QueryInfo.WhereInjectKeyword))
            {
                where = _QueryInfo.WhereInjectKeyword + (string.IsNullOrEmpty(where)? "": " AND " + where);
            }

            var q = selectSql + (String.IsNullOrEmpty(where)? "" : " WHERE " + where);

            return q;
        }

        void AddRefTable(IEntityRelation rel)
        {
            var idf = "_" + idx;
            var refEnt = _QueryInfo.Entity.GetEntity(rel.ChildName);
            var shouldJoin = false;
                if (rel.Type == EntityRelationType.ManyToOne)
                {
                    AddSelectField(rel.ParentRefField.Name, $"{idf}.{rel.ChildDisplayField.DBName}", $"{rel.ParentRefField.Name}__name");                    

                    shouldJoin = true;
                }
                else if (rel.Type == EntityRelationType.ManyToMany)
                {
                    AddSelectField(rel.ParentRefField.Name, $"get_related_data('{refEnt.DBName}', '{rel.ChildDisplayField.DBName}', {_entTable}.{rel.ParentRefField.DBName})", $"{rel.ParentRefField.Name}__data");
                }                

                if (shouldJoin)
                {   
                    if (!_toJoinTables.ContainsKey(rel.ParentRefField.Name))                 
                    {
                        _toJoinTables.Add(rel.ParentRefField.Name, 
                            (refEnt.DBName, idf.ToString(), new List<string>() { $"{_entTable}.{rel.ParentRefField.DBName} = {idf}.id"} ));
                    }
                }

            idx++;
        }

        private void AddCollectionTable(BaseField fieldSchema)
        {
            var collectionInfo = ((SelectField)fieldSchema).ControlInfo.CollectionInfo;
            var collId = collectionInfo.Id;

            if ((int)collectionInfo.SourceType == 0)
            {
                var idf = "_" + idx;

                if (fieldSchema is MultiSelectField)
                {
                    AddSelectField(fieldSchema.Name, $"get_collection_datatext({collId}, {_entTable}.{fieldSchema.DBName})", $"{fieldSchema.Name}__data");
                }
                else
                {
                    AddSelectField(fieldSchema.Name, $"{idf}.datatext", $"{fieldSchema.Name}__name");

                    _toJoinTables.Add(fieldSchema.Name, 
                        ("t_collection_master", idf.ToString(),
                            new List<string>() { 
                                $"{idf}.id = {collId}",
                                $"{_entTable}.{fieldSchema.DBName} = {idf}.dataid"
                            } 
                    ));
                    idx++;
                }
            }
        }

        private void AddSelectField(string fieldName, string queryName, string queryAlias)
        {
            if (_selectFields.Where(x => x.Item3.ToLower() == queryAlias.ToLower()).Count() == 0)
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
                foreach(var k in _toJoinTables)
                {
                    var x = k.Value;
                    qry += $" LEFT JOIN {x.Item1} as {x.Item2} ON {string.Join(" AND ", x.Item3)}";
                }
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

        public static string PrepareWhereExp(DbQuery qInfo, string entTable, FilterExpression fixedFilter, FilterExpression filters)
        {
            var commonexp = qInfo.IncludeGlobalMasterId ? $"{entTable}.masterid in (@MasterId, 0)": $"{entTable}.masterid = @MasterId";
            var whereFix = PrepareFilterSql(qInfo, fixedFilter);

            var whereQ = PrepareFilterSql(qInfo, filters);

            if (string.IsNullOrEmpty(whereFix))
                return commonexp + (string.IsNullOrEmpty(whereFix) ? "" : " AND " + whereQ);

            return commonexp + " AND " +  whereFix + (String.IsNullOrEmpty(whereQ) ? "": $" AND ({whereQ})"); 
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
                if (filter.Value != null && filter.Value is Array)
                {
                    //String[] vals = filter.Value.ToString().Split(',');
                    foreach(var d in (object[])filter.Value)
                    {
                        v1.Add(GetSqlVal(dbField, d.ToString()));
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