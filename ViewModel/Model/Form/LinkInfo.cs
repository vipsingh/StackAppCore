using System;
using StackErp.Model;

namespace StackErp.ViewModel.Model
{
    public class LinkInfo
    {
        public string Action {set;get;}
        public RequestQueryString Query {set;get;}
        public bool IsRedirect {set;get;}
        public LinkInfo(string url)
        {
            this.Action = url;
        }

        public string ToURL()
        {
            var str = this.Action;
            if (this.Query != null) 
            {
            str += $"?{this.Query.ToQueryString()}";
            }

            return str;
        }
    }
}