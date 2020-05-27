using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackErp.Model.Entity;
using StackErp.Model.Utils;

namespace StackErp.Model.Layout
{
    public class TListInfo
    {
        public int Entity {set;get;}
        public string IdField {set;get;}
        public string LinkField {set;get;}
        public List<TListField> Select {set;get;}
        public List<string> Additional {set;get;}
        public JObject Where {set;get;}
        public List<string> OrderBy {set;get;}
    }

    // public class TListField
    // {
    //     public string FieldName {set;get;}
    //     public string Format {set;get;}
    //     public bool Link {set;get;}
    // }
}