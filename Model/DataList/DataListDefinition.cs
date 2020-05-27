using System;
using System.Collections.Generic;
using StackErp.Model.Entity;
using StackErp.Model.Layout;

namespace StackErp.Model.DataList
{
    public class DataListDefinition
    {
        public FieldDataSource DataSource {set;get;}
        //public bool IsNonEntity {set;get;}
        //public EntityCode EntityId {set;get;}
        public string Id {set;get;}
        public string Name {set;get;}
        public string ItemIdField {set;get;}
        public TList Layout  {set;get;}
        public TListInfo ListLayout  {set;get;}
        public int PageSize {set;get;}
        public string ItemViewField {set;get;}
        public List<string> OrderByField {set;get;}
        public List<string> AdditionalFields {set;get;}
        public FilterExpression FixedFilter {set;get;}
        public FilterExpression FilterPolicy {set;get;}
    }

    public class EntityListDefinition: DataListDefinition
    {
        public EntityCode EntityId {set;get;}
    }
}
