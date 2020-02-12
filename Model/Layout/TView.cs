using System;
using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json;
using StackErp.Model.Entity;
using StackErp.Model.Utils;

namespace StackErp.Model.Layout
{

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
            var fields = new List<TField>();
            
            ExtractPageFields(this.Header, ref fields);
            
            foreach (var p in this.Pages)
            {
                ExtractPageFields(p, ref fields);
            }
            return fields;
        }
        private void ExtractPageFields(TPage p, ref List<TField> fields)
        {
            if (p == null) return;
            
            foreach (var g in p.Groups)
                {
                    foreach (var r in g.Rows)
                    {
                        fields.AddRange(r.Fields);
                    }
                }
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
        [JsonIgnore]
        public string FieldName { set; get; }
        public string FieldId { set; get; }
        public string Text { set; get; }
        public bool FullRow { set; get; }
        public string InVisible { set; get; }
        [JsonIgnore]
        public string Domain { set; get; }
        [JsonIgnore]
        public FormControlType Widget { set; get; }
        public static TField FromXmlNode(XmlNode node)
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

    public class TList
    {
        public string Id  { set; get; }
        public string Text  { set; get; }
        public List<TField> Fields { set; get; }
        public TList()
        {
            Fields = new List<TField>();
        }
    }
}
