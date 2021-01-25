using System;
using System.Collections.Generic;
using System.Linq;
using StackErp.Model.Entity;
using StackErp.Model.Utils;

namespace StackErp.Model.Layout
{

    public class TList
    {
        public string Id  { set; get; }
        public string Text  { set; get; }
        public string ViewType  { set; get; }
        public List<TRow> RowTemplate { set; get; }
        public TCalendar Calendar { set; get; }
        public TKanban Kanban { set; get; }
        public List<TListField> Fields { set; get; }
        public List<TFormRule> Rules { set; get; }
        public TList()
        {
            Fields = new List<TListField>();
        }

        public List<TListField> GetLayoutFields()
        {
            return this.Fields;
        }

        // public static TList ParseFromXml(string xml)
        // {
        //     var ser = new LayoutViewSerialzer();
        //     return ser.SerializeTList(xml);
        // }

        public static TList ParseFromJSON(string json)
        {
           return Newtonsoft.Json.JsonConvert.DeserializeObject<TList>(
                json, 
                new Newtonsoft.Json.JsonSerializerSettings { ContractResolver = new LayoutJsonContractResolver() });
        }
    }

    public class TListField: TField
    {
        public bool AllowOrder {set;get;}
    }

    public class TCalendar
    {
        public string StartDateField {set;get;}
        public string EndDateField {set;get;}
    }

    public class TKanban
    {
        public string BoardField {set;get;}
        public List<string> Boards {set;get;}

    }
    
}