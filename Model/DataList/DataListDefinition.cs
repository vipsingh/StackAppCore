using System;
using StackErp.Model.Entity;
using StackErp.Model.Layout;

namespace StackErp.Model.DataList
{
    public class DataListDefinition
    {
        public bool IsNonEntity {set;get;}
        public EntityCode EntityId {set;get;}
        public string Id {set;get;}
        public string ItemIdField {set;get;}
        public TList Layout  {set;get;}
        public int PageSize {set;get;}
    }
}
