using System;
using System.Collections.Generic;
using StackErp.Model.DataList;

namespace StackErp.Model.Entity
{
    public class EntityQuery
    {
        public string EntityName {private set;get;}
        public QueryType QueryType {private set;get;}
        public EntityQueryFieldCollection Fields {private set;get;}
        public FilterExpression Filters {private set;get;}

        public List<(string, string)> OrderBy {private set;get;}
        public string ItemIdField {private set;get;}
        public string PageSize {private set;get;}
        protected int PageIndex;

        public EntityQuery(string entityName) {
            EntityName = entityName;
        }

        public void WithPage(int pageIndex) 
        {
            PageIndex= pageIndex;
        }
    }

    public class EntityQueryFieldCollection: List<EntityQueryField>
    {

    }
    public class EntityQueryField {
        public string EntityName {private set;get;}
        public string FieldName {private set;get;}
        public BaseField Field {private set;get;}
        public bool IsSelect {private set;get;}
        public string Alias {private set;get;}        
        public string Format {private set;get;}
        public BaseTypeCode DataType {private set;get;}
    }
}

/*

*/