using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using StackErp.Model.Entity;
using StackErp.Model.Utils;
using Newtonsoft.Json.Serialization;

namespace StackErp.Model.Layout
{

    public class TView
    {
        public TPage Header { set; get; }
        public int RenderingStyle { set; get; }
        public List<TPage> Pages { set; get; }      

        [JsonIgnore]
        public List<TField> Fields { set; get; }  
        public List<TCommand> Commands { set; get; }
        public TView()
        {
            Pages = new List<TPage>();
        }

        public TField GetField(string id) {
            return Fields.Find(f => f.FieldId == id);
        }
        public List<TField> GetAllFields()
        {
            // var fields = new List<TField>();
            
            // ExtractPageFields(this.Header, ref fields);
            
            // foreach (var p in this.Pages)
            // {
            //     ExtractPageFields(p, ref fields);
            // }
            return Fields;
        }

        public void ClearBlankRows()
        {
            if (Header != null && Header.Groups != null) {
                foreach(var g in Header.Groups)
                {
                    ClearBlankRowsFromGroup(g);
                }
            }

            foreach(var p in Pages)
            {
                if (p.Groups != null) {
                    foreach(var g in p.Groups)
                    {
                        ClearBlankRowsFromGroup(g);
                    }
                }
            }
        }

        private void ClearBlankRowsFromGroup(TGroup group)
        {
            if (group == null || group.Rows.Count == 0) return;

            var list = new List<int>();
            for(int i=0; i<group.Rows.Count; i++)
            {
                var r = group.Rows[i];
                if (r.Cols.Count == 0 || r.Cols.Where(c => string.IsNullOrEmpty(c.Id)).Count() == r.Cols.Count)
                    list.Add(i);
            }

            foreach(var i in list)
                group.Rows.RemoveAt(i);            
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

        public static TView ParseFromXml(string xml)
        {
            var ser = new LayoutViewSerialzer();
            return ser.SerializeTView(xml);
        }

        public static TView ParseFromJSON(string json) {
            if (string.IsNullOrEmpty(json)) return null;
            
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TView>(
                json, 
                new Newtonsoft.Json.JsonSerializerSettings { ContractResolver = new LayoutJsonContractResolver() });

        }

        public string ToStoreJSON() {

            return Newtonsoft.Json.JsonConvert.SerializeObject(
                this, 
                new Newtonsoft.Json.JsonSerializerSettings { ContractResolver = new LayoutJsonContractResolver() });

        }
    }
    public class TRow
    {
        public TRow()
        {
            this.Fields = new List<TField>();
            this.Cols = new List<TCol>();
        }
        public List<TField> Fields { set; get; }

        public List<TCol> Cols { set; get; }
    }

    public class TCol {

        public TCol(string id, string type = "FIELD") {
            Id =id;
            Type = type;
        }   
        public string Id { set; get; }
        public string Type { set; get; }
        public int Span { set; get; }
    }
    public class TField
    {
        public string ViewName { set; get; }
        public string FieldId { set; get; }
        public string Text { set; get; }
        public bool FullRow { set; get; }
        public string Invisible { set; get; }
        public string IsMandatory { set; get; } //ScriptType<bool>
        public string ReadOnly { set; get; }
        public string Domain { set; get; }
        public FormControlType WidgetType { set; get; }
        public int CaptionPosition { set; get; }
        public string Format { set; get; }
        public string Width { set; get; }
        public DynamicObj Style { set; get; }
        public static TField FromXmlNode(XmlNode node)
        {
            var f = new TField();
            var attrs = node.Attributes;

            Guard.Against.Null(attrs["id"], "FieldId");
            f.FieldId = attrs["id"].Value;

            if (attrs["invisible"] != null) 
            {
                f.Invisible = attrs["invisible"].Value.Trim();                
            }
            if (attrs["readonly"] != null) 
            {
                f.ReadOnly = attrs["readonly"].Value.Trim();                
            }

            f.FullRow = GetXmlAttrValue(attrs, "fullrow", false);
            f.Text = GetXmlAttrValue(attrs, "text", "");
            f.WidgetType = GetXmlAttrValue(attrs, "widget", FormControlType.None);
            f.Format = GetXmlAttrValue(attrs, "format", "");

            return f;
        }

        private static T GetXmlAttrValue<T>(XmlAttributeCollection collection, string name, T defaultvalue)
        {
            if (collection[name] != null)
                return DataHelper.GetData(collection[name].Value.Trim(), defaultvalue);    
            
            return defaultvalue;
        }
    }

    public class TGroup
    {
        public string Id { set; get; }
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
        public string Id { set; get; }
        public string Text { set; get; }
        public List<TGroup> Groups { set; get; }
        public TPage()
        {
            this.Groups = new List<TGroup>();
        }
    }

    public class TFormRule
    {
        public string Type {set;get;}
        public string Criteria {set;get;}
        public string Style {set;get;}
        public List<string> Fields {set;get;}
    }
    
    public class TCommand
    {
        [XmlAttribute]
         public int Id {set;get;}
         [XmlAttribute]
        public string Text {set;get;}
        [XmlAttribute]
        public string Position {set;get;}
    }    

    public class LayoutJsonContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, Newtonsoft.Json.MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            property.Ignored = false; // Here is the magic
            return property;
        }
    }
}
