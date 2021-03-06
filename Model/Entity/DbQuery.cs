using System;
using System.Collections.Generic;
using StackErp.Model.DataList;
using StackErp.Model.Layout;

namespace StackErp.Model.Entity
{
    public class DbQuery
    {
        public EntityCode EntityId {get;}
        public IDBEntity Entity {get;}
        public bool IncludeGlobalMasterId {private set; get;}
        public QueryType QueryType {private set;get;}
        public DbQueryFieldCollection Fields {get;}
        public string WhereInjectKeyword {set;get;}
        public FilterExpression FixedFilter {private set;get;}
        public FilterExpression Filters {private set;get;}

        public List<(string, string)> OrderBy {private set;get;}
        public string ItemIdField {set;get;}
        public int PageSize {private set;get;}
        protected int PageIndex;
        public bool IsFixedQuery {private set;get;}
        public string QrySql {private set;get;}
        public List<IEntityRelation> Relations {get;}

        public DbQuery(IDBEntity entity, bool includeGlobalMasterId = false) {
            Entity = entity;
            EntityId = entity.EntityId;
            Fields =  new DbQueryFieldCollection();            
            Relations  = Entity.Relations;
            IncludeGlobalMasterId = includeGlobalMasterId;
        }

        public void BuildWithListDefn(DataListDefinition defn)
        {
            ItemIdField = defn.ItemIdField;
            
            var view  = defn.Layout;
            foreach(var f in view.GetLayoutFields())
            {
                AddField(f.FieldId, true);
            }

            AddField(ItemIdField, false);

            if (defn.FixedFilter != null)
                FixedFilter = defn.FixedFilter.DeepClone();
            
            if (defn is EntityListDefinition)
            {
                IncludeGlobalMasterId = ((EntityListDefinition)defn).IncludeGlobalMasterId;
            }
            else if (defn.DataSource != null) {
                IncludeGlobalMasterId = defn.DataSource.IncludeGlobalMasterId;
            }
        }

        // public void BuildWithListInfo(TListInfo view)
        // {
        //     ItemIdField = view.IdField;

        //     foreach(var f in view.Select)
        //     {
        //         AddField(f.FieldName, true);
        //     }
        //     var filterJson = view.Where;
        //     if (filterJson != null)
        //     {
        //         FixedFilter = FilterExpression.BuildFromJson(view.Entity, filterJson.ToString());
        //     }
        // }

        public void AddField(string fieldName, bool isSelect = true)
        {
            var f =  new DbQueryField(fieldName, isSelect);
            Fields.AddField(f);
        }

        private bool _isresolved = false;
        public void ResolveFields()
        {
            if (_isresolved) return;
            
            if(FixedFilter != null)
            {
                foreach(var fl in FixedFilter.GetAll())
                {
                    var f =  new DbQueryField(fl.FieldName, false);
                    Fields.AddField(f);
                }
            }

            if(Filters != null)
            {
                foreach(var fl in Filters.GetAll())
                {
                    var f =  new DbQueryField(fl.FieldName, false);
                    Fields.AddField(f);
                }
            }

            foreach(DbQueryField f in Fields)
            {
                var fSchema = Entity.GetFieldSchema(f.FieldName);
                f.Field = fSchema;
                f.DbName = fSchema.DBName;
                f.Alias = Entity.DBName;
                f.TableName = Entity.DBName;
            }
            
            _isresolved =true;
        }

        public void SetFilter(FilterExpression filter)
        {
            Filters = filter;
        }

        public void SetFixedFilter(FilterExpression filter)
        {
            FixedFilter = filter;
        }

        public void WithPage(int pageIndex, int pageSize)
        {
            PageIndex= pageIndex;
            PageSize = pageSize;
        }
    }

    public class DbQueryFieldCollection: List<DbQueryField>
    {
        public void AddField(DbQueryField field)
        {
            if(this.FindAll(x => x.FieldName == field.FieldName).Count == 0)
            {
                this.Add(field);
            }
        }

        // public DbQueryField GetByName(string fieldName)
        // {
            
        // }
    }
    public class DbQueryField {
        public DbQueryField(String fieldName, bool isSelect = true) 
        {
            FieldName = fieldName;
            IsSelect = isSelect;
        }

        public string EntityName {set;get;}
        public string FieldName {set;get;}
        public BaseField Field {set;get;}
        public bool IsSelect {set;get;}
        public string Alias {set;get;}   
        public string DbName {set;get;}
        public string TableName {set;get;}
        public string Format {set;get;}
        public TypeCode DataType {set;get;}
        public string SummaryType {set;get;}
    }
}

/*
{
  "Entity": 111,
  "IdField": "id",
  "LinkField": "name",
  "Select": [
    {"FieldName": "name", "Format": "", "Link": false}
    {"FieldName": "role.name"}
    ],
  "Additional": [],
  "Where": {
    "$or": [
      {
        "status": [0, 1]
      }
    ]
  },
  "OrderBy": ["name"]
}
*/