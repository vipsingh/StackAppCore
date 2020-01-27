using System;
using StackErp.Model.Entity;
using StackErp.Model.Layout;

namespace StackErp.Model.DataList
{
    public class DataListDefinition
    {
        public string Id {set;get;}
        public TList Layout  {set;get;}
        public DbQuery Query {set;get;}
    }
}
