using System;
using System.Collections.Generic;

namespace StackErp.Model.DataList
{    
    public class QueryRequest
    {
        public int PageIndex {set;get;}
        public int PageCount {set;get;}
        public int PageSize {set;get;}

        public List<string> OrderBy {set;get;}
        public string Search  {set;get;}
        public object DataFilter {set;get;}
    }

    public class DataListOutput
    {
        public dynamic Data {set;get;}
        public int TotalCount {set;get;}
    }
}
