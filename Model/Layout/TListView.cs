using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using StackErp.Model.Entity;
using StackErp.Model.Utils;

namespace StackErp.Model.Layout
{

    public class TList
    {
        public string Id  { set; get; }
        public string Text  { set; get; }
        public string Type  { set; get; }
        public List<TField> Fields { set; get; }
        public List<TFormRule> Rules { set; get; }
        public TList()
        {
            Fields = new List<TField>();
        }

        public List<TField> GetLayoutFields()
        {
            return this.Fields;
        }

        public static TList ParseFromXml(string xml)
        {
            var ser = new LayoutViewSerialzer();
            return ser.SerializeTList(xml);
        }
    }

    public class TListField: TField
    {
        public bool AllowOrder {set;get;}
    }

    internal class LayoutViewSerialzer
    {
        internal TView SerializeTView(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);

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

            view.FormRules = new List<TFormRule>();

            var rules = root.SelectSingleNode("/view/rules");
            if (rules != null) 
            {
                foreach (XmlNode r in rules.SelectNodes("./rule"))
                {
                    var fr = new TFormRule();
                    fr.Criteria = r.Attributes["criteria"].Value;
                    fr.Type = r.Attributes["type"].Value;
                    var rFields = r.Attributes["fields"].Value;
                    if (!String.IsNullOrEmpty(rFields))
                    {
                        fr.Fields = rFields.Split(',').ToList();
                    }
                    view.FormRules.Add(fr);
                }
            }

            var commands = root.SelectSingleNode("/view/commands");
            if (commands != null) 
            {       
                view.Commands = new List<TCommand>();         
                foreach (XmlNode r in commands.SelectNodes("./command"))
                {
                    var fr = new TCommand();
                    fr.Id = ReadAttribute(r, "id");
                    fr.Text = ReadAttribute(r, "text");
                    fr.Position = ReadAttribute(r, "position");                    
                    view.Commands.Add(fr);
                }
            }

            return view;
        }

        private static string ReadAttribute(XmlNode node, string attr)
        {
            var n = node.Attributes[attr];

            return n== null ? null: n.Value;
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

        internal TList SerializeTList(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);

            return CreateListView(document);
        }

        private TList CreateListView(XmlDocument document)
        {
            var view = new TList();

            XmlNode root = document.FirstChild;
            var v = root.SelectSingleNode("/tlist");

            var fList = root.SelectNodes("/tlist/template/row/field");
            foreach (XmlNode page in fList)
            {
                view.Fields.Add(TField.FromXmlNode(page));
            }

            return view;
        }

    }
}