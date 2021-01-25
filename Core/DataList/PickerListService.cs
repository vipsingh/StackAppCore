using System;
using System.Collections;
using System.Collections.Generic;
using StackErp.Core.Layout;
using StackErp.DB.DataList;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;
using StackErp.Model.Layout;

namespace StackErp.Core.DataList
{
    public class PickerListService: EntityListService
    {
        public DataListDefinition GetListDefn(BaseField field)
        {
            var defn = GetEntityListDefn(field.ControlInfo.DataSource);
            return defn;
        }

        public DataListDefinition GetListDefn(FieldDataSource pickerSource)
        {
            var defn = GetEntityListDefn(pickerSource);
            return defn;
        }

        public DataListDefinition GetEntityListDefn(FieldDataSource source)
        {
            var defn = new DataListDefinition();
            var ds = source;
            defn.DataSource = ds;

            var _Entity = Core.EntityMetaData.Get(ds.Entity);
            
            defn.ItemIdField = "Id";
            defn.ItemViewField = _Entity.GetFieldSchema(_Entity.TextField).ViewName;
            
            defn.Layout = PrepareLayout(defn, _Entity);
            defn.PageSize = 25;
            if (ds.Domain != null)
            {
                defn.FixedFilter = ds.Domain;
            }
            
            return defn;
        }


        private TList PrepareLayout(DataListDefinition ds, IDBEntity entity)
        {
            var fields = new String[] { ds.ItemIdField, ds.ItemViewField };
            var tList = new TList();
            tList.Fields = new List<TListField>();
            foreach(var f in fields)
            {
                var tfield = new TListField()
                {
                    FieldId = f
                };
                 tList.Fields.Add(tfield);
            }            

            return tList;
        }        
    }
}
