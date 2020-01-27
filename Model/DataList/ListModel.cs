using System;
using System.Collections.Generic;

namespace StackErp.Model.DataList
{
    public class DataListSchema
    {
        public string Id {set;get;}
        public EntityCode EntityId {set;get;}
        public List<DataListColumn> Columns {set;get;}
        public string RenderMode{set;get;}
        public string Caption {set;get;}
        public string IdColumn {set;get;}
        public object Template{set;get;}
    }
    
    public class DataListColumn
    {
        public FieldType FieldType {set;get;}
        public string Name {set;get;}
        public BaseTypeCode BaseType {set;get;}
        public string Caption {set;get;}
        public string IsHidden {set;get;}
        public int width {set;get;}        
        public string Help {set;get;}
        public string FormatInfo {set;get;}
    }

    public class DataListRow
    {
        public int RowId {set;get;}
        public dynamic Data {set;get;}
    }
}
