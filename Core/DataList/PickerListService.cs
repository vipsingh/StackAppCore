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
            // var layout = new EntityLayoutService(null, entityId);
            var defn = new DataListDefinition();
            // defn.Id = entityId.Code.ToString() + "_" + queryId.ToString();
            // defn.Layout = layout.PrepareListLayout(queryId);
            // defn.Query = null;
            var ds = field.ControlInfo.DataSource;
            defn.EntityId = ds.Entity;
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
                tList.Fields.Add(new TField(){ FieldName = f });
            }

            return tList;
        }        
    }
}
