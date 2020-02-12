using System;
using System.Collections.Generic;
using System.IO;
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
        public EntityLayoutService(StackAppContext appContext, EntityCode entityId)
        {
            _EntityId = entityId;
        }

        public TView PrepareView(int layoutId)
        {
            var layout = LayoutDbService.GetItemType(_EntityId.Code, layoutId);
            if (layout == null)
                throw new EntityException("Layout is not defined.");

            var layoutXml = layout.Get("layoutxml", string.Empty);
            if (string.IsNullOrWhiteSpace(layoutXml))
                return CreateDefault();

            XmlDocument document = new XmlDocument();
            document.LoadXml(layoutXml);

            return Create(document);
        }

        private TView Create(XmlDocument document)
        {
            var view = new TView();

            XmlNode root = document.FirstChild;
            var header = root.SelectSingleNode("/view/header");

            var h = new TPage();
            if (header.Attributes["text"] != null)
                h.Text = header.Attributes["text"].Value;

            foreach (XmlNode group in header.SelectNodes("./group"))
            {
                var g = CreateGroup(group);
                h.Groups.Add(g);
            }

            view.Header = h;


            var pageList = root.SelectNodes("/view/pages/page");
            foreach (XmlNode page in pageList)
            {
                var p = new TPage();
                if (page.Attributes["text"] != null)
                    p.Text = page.Attributes["text"].Value;

                foreach (XmlNode group in page.SelectNodes("./group"))
                {
                    var g = CreateGroup(group);
                    p.Groups.Add(g);
                }

                view.Pages.Add(p);
            }




            return view;
        }

        private TGroup CreateGroup(XmlNode group)
        {
            var g = new TGroup();
            if (group.Attributes["text"] != null)
                g.Text = group.Attributes["text"].Value;

            foreach (XmlNode row in group.SelectNodes("./row"))
            {
                var r = new TRow();
                foreach (XmlNode field in row.SelectNodes("./field"))
                {
                    r.Fields.Add(TField.FromXmlNode(field));
                }
                g.Rows.Add(r);
            }
            return g;
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
                field.FieldId = f.ViewName;
                field.FieldName = f.Name;

                if (col_r.Count == 1)
                    start_new_r = true;
                else
                    start_new_r = false;
                col_r.Add(field);

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

        public TList PrepareListLayout(int listId)
        {
            // var layout = LayoutDbService.GetItemType(_EntityId.Code, layoutId);
            // if (layout == null)
            //     throw new EntityException("Layout is not defined.");

            var layoutXml = String.Empty; //layout.Get("layoutxml", string.Empty);
            if (string.IsNullOrWhiteSpace(layoutXml))
                return CreateDefaultList();
           
            XmlDocument document = new XmlDocument();
            document.LoadXml(layoutXml);

            var view = new TList();

            XmlNode root = document.FirstChild;
            var v = root.SelectSingleNode("/tlist");

            var fList = root.SelectNodes("/tlist/field");
            foreach (XmlNode page in fList)
            {
                view.Fields.Add(TField.FromXmlNode(page));
            }

            return view;
        }

        public TList CreateDefaultList() {
            var _Entity = Core.EntityMetaData.Get(_EntityId);
            var layoutF = _Entity.GetLayoutFields(EntityLayoutType.View);

            var view = new TList();
            view.Text = _Entity.Text;

            foreach (var f in layoutF)
            {
                var field = new TField();
                field.FieldId = f.ViewName;
                field.FieldName = f.Name;
                view.Fields.Add(field);
            }
            
            return view;
        }
    }
}
