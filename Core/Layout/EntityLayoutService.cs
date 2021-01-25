using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using StackErp.DB.Layout;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Utils;
using StackErp.Model.Layout;

namespace StackErp.Core.Layout
{
    public class EntityLayoutService
    {
        private EntityCode _EntityId;
        private StackAppContext _AppContext;
        public EntityLayoutService(StackAppContext appContext, EntityCode entityId)
        {
            _EntityId = entityId;
            _AppContext = appContext;
        }

        public TView PrepareView(int itemTypeId, EntityLayoutType layoutType)
        {
            string layoutJson = "";
            if (itemTypeId != 0)
            {
                var layout = LayoutDbService.GetItemTypeLayout(_AppContext.MasterId, _EntityId.Code, itemTypeId, (int)layoutType);
                if (layout == null)
                    throw new EntityException("ItemType is not defined.");

                //layoutXml = layout.Get("layoutxml", string.Empty);
                layoutJson = layout.Get("layoutjson", string.Empty);
            }

            if (string.IsNullOrWhiteSpace(layoutJson))
            {
                var _Entity = Core.EntityMetaData.Get(_EntityId);            
                var lview =  _Entity.GetDefaultLayoutView(layoutType);
                return lview;
            }
            
            return TView.ParseFromJSON(layoutJson);
        }       

        public static TView CreateDefault(IDBEntity Entity, EntityLayoutType layoutType)
        {

            var view = new TView();
            view.Fields = new List<TField>();

            var page = new TPage();
            view.Pages.Add(page);
            var group = new TGroup();
            page.Groups.Add(group);            

            var start_new_r = true;
            List<TCol> col_r = new List<TCol>();
            var layoutF = Entity.GetLayoutFields(EntityLayoutType.Edit);
            int idx = 0;
            foreach (var f in layoutF)
            {
                var col = new TCol(f.Name);                

                view.Fields.Add(new TField() { FieldId = f.Name, Text = f.Text });

                if (col_r.Count == 1)
                    start_new_r = true;
                else
                    start_new_r = false;

                if (IsWidgetOnFullRow(f.ControlType))
                {
                    start_new_r = true;
                    
                    var row = new TRow();
                    col.Span = 24;
                    row.Cols = new List<TCol>() { col };
                    
                    group.Rows.Add(row);
                }
                else {
                    col_r.Add(col);
                } 
                

                if (start_new_r)
                {
                    var row = new TRow();
                    row.Cols = col_r;
                    group.Rows.Add(row);

                    col_r = new List<TCol>();
                }
                else if (layoutF.Count - 1 == idx)
                {
                    var row = new TRow();
                    row.Cols = col_r;

                    group.Rows.Add(row);
                }
                idx++;
            }

            return view;
        }

        public static bool IsWidgetOnFullRow(FormControlType ctrlType)
        {
            switch(ctrlType) {
                case FormControlType.XmlEditor:
                case FormControlType.ListForm:
                case FormControlType.EntityListView:
                case FormControlType.HtmlText:
                    return true;
                default:
                    return false;
            }
        }
    }    
}
