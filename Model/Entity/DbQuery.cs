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
        public QueryType QueryType {private set;get;}
        public DbQueryFieldCollection Fields {get;}
        public FilterExpression Filters {private set;get;}

        public List<(string, string)> OrderBy {private set;get;}
        public string ItemIdField {set;get;}
        public int PageSize {private set;get;}
        protected int PageIndex;
        public bool IsFixedQuery {private set;get;}
        public string QrySql {private set;get;}
        public List<IEntityRelation> Relations {get;}

        public DbQuery(EntityCode entityId) {
            EntityId = entityId;
            Fields =  new DbQueryFieldCollection();            
        }
        public DbQuery(IDBEntity entity): this(entity.EntityId) {
            Entity = entity;
            Relations  = Entity.Relations;
        }

        public void BuildWithLayout(TList view)
        {
            foreach(var f in view.Fields)
            {
                AddField(f.FieldId, true);
            }
        }

        public void AddField(string fieldName, bool isSelect = true)
        {
            var f =  new DbQueryField(fieldName, isSelect);
            Fields.AddField(f);
        }

        public void ResolveFields()
        {
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
            }
        }

        public void SetFilter(FilterExpression filter)
        {
            Filters = filter;
        }

        public void AddFixedFilter()
        {

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
        public string Format {set;get;}
        public BaseTypeCode DataType {set;get;}
        public string SummaryType {set;get;}
    }
}

/*

*/