using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using StackErp.DB.Layout;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Utils;

namespace StackErp.Core.Layout
{
    public class EntityLayoutService
    {
        private IDBEntity _Entity;
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
            if(string.IsNullOrWhiteSpace(layoutXml))
                return CreateDefault();

            XmlDocument document = new XmlDocument();
            document.LoadXml(layoutXml);

            return Create(document);
        }

        private TView Create(XmlDocument document)
        {
            var view = new TView();

            XmlNode root = document.FirstChild;
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
    }

    public class TView
    {
        public TPage Header { set; get; }
        public int RenderingStyle { set; get; }
        public List<TPage> Pages { set; get; }
        public TView()
        {
            Pages = new List<TPage>();
        }
        public List<TField> GetAllFields()
        {
            var fields = new  List<TField>();
            foreach(var p in this.Pages)
            {
                foreach(var g in p.Groups)
                {
                    foreach(var r in g.Rows)
                    {
                        fields.AddRange(r.Fields);
                    }
                }
            }
            return fields;
        }
    }
    public class TRow
    {
        public TRow()
        {
            this.Fields = new List<TField>();
        }
        public List<TField> Fields { set; get; }
    }
    public class TField
    {
        public string FieldId { set; get; }
        public string Text { set; get; }
        public bool FullRow { set; get; }
        public string InVisible { set; get; }
        public string Domain { set; get; }
        internal static TField FromXmlNode(XmlNode node)
        {
            var f = new TField();
            var attrs = node.Attributes;

            Guard.Against.Null(attrs["id"], "FieldId");
            f.FieldId = attrs["id"].Value;

            return f;
        }
    }

    public class TGroup
    {
        public string Text { set; get; }
        public string Style { set; get; }
        public List<TRow> Rows { set; get; }
        public TGroup()
        {
            this.Rows = new List<TRow>();
        }
    }

    public class TPage
    {
        public string Text { set; get; }
        public List<TGroup> Groups { set; get; }
        public TPage()
        {
            this.Groups = new List<TGroup>();
        }
    }

}
