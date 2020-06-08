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
    
}