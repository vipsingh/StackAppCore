using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using StackErp.Model.Form;

namespace StackErp.Model.DataList
{    
    public class QueryRequest
    {
        public int PageIndex {set;get;}
        public int PageCount {set;get;}
        public int PageSize {set;get;}

        public List<string> OrderBy {set;get;}
        public string Search  {set;get;}
        public Dictionary<string, UIFormField> FilterModel {set;get;}
        public JObject DataFilter {set;get;}
    }

    public class DataListOutput
    {
        public dynamic Data {set;get;}
        public int TotalCount {set;get;}
    }
}
