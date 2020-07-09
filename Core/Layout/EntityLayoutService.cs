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
using System.Xml.Serialization;

namespace StackErp.Core.Layout
{
    public class EntityLayoutService
    {
        private EntityCode _EntityId;
        public EntityLayoutService(StackAppContext appContext, EntityCode entityId)
        {
            _EntityId = entityId;
        }

        public TView PrepareView(int itemTypeId, EntityLayoutType layoutType)
        {
            string layoutXml = "";
            if (itemTypeId != 0)
            {
                var layout = LayoutDbService.GetItemTypeLayout(_EntityId.Code, itemTypeId, (int)layoutType);
                if (layout == null)
                    throw new EntityException("ItemType is not defined.");

                layoutXml = layout.Get("layoutxml", string.Empty);
            }

            if (string.IsNullOrWhiteSpace(layoutXml))
                return CreateDefault();

            return TView.ParseFromXml(layoutXml);
        }       

        private TView CreateDefault()
        {
            var view = new TView();
            var page = new TPage();
            view.Pages.Add(page);
            var group = new TGroup();
            page.Groups.Add(group);

            var _Entity = Core.EntityMetaData.Get(_EntityId);

            var start_new_r = true;
            List<TField> col_r = new List<TField>();
            var layoutF = _Entity.GetLayoutFields(EntityLayoutType.Edit);
            int idx = 0;
            foreach (var f in layoutF)
            {
                var field = new TField();
                field.FieldId = f.Name;

                if (col_r.Count == 1)
                    start_new_r = true;
                else
                    start_new_r = false;

                if (IsWidgetOnFullRow(f.Type))
                {
                    start_new_r = true;
                    var row = new TRow();
                    field.FullRow = true;
                    row.Fields = new List<TField>() { field };
                    group.Rows.Add(row);
                }
                else {
                    col_r.Add(field);
                } 
                

                if (start_new_r)
                {
                    var row = new TRow();
                    row.Fields = col_r;
                    group.Rows.Add(row);
                    col_r = new List<TField>();
                }
                else if (layoutF.Count - 1 == idx)
                {
                    var row = new TRow();
                    row.Fields = col_r;
                    group.Rows.Add(row);
                }
                idx++;
            }

            return view;
        }

        private bool IsWidgetOnFullRow(FieldType fieldType)
        {
            if (fieldType == FieldType.OneToMany || fieldType == FieldType.OneToOne)
                return true;

            return false;
        }
    }
}
