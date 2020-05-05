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
            var defn = new DataListDefinition();            
            var ds = field.ControlInfo.DataSource;
            defn.EntityId = ds.Entity;
            var _Entity = Core.EntityMetaData.Get(ds.Entity);
            
            defn.ItemIdField = "Id";
            defn.ItemViewField = _Entity.GetFieldSchema(_Entity.TextField).ViewName;
            defn.Id = "_" + field.Name;
            defn.Layout = PrepareLayout(ds);
            defn.PageSize = 25;

            return defn;
        }

        private TList PrepareLayout(PickerDataSource ds)
        {
            var tList = new TList();
            foreach(var f in ds.Fields)
            {
                tList.Fields.Add(new TField(){ FieldId = f });
            }

            return tList;
        }        
    }
}
