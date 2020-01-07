using System;

namespace StackErp.Model.Form
{
    public class ActionInfo
    {
        public string ActionId { set; get; }
        public string Title { set; get; }
        public string Url { set; get; }
        public string RawUrl { set; get; }
        public RequestQueryString Query { set; get; }
        public ActionType ActionType { set; get; }
        public ActionButtonPosition ActionPosition { set; get; }
        public ActionDisplayType DisplayType { set; get; }
        public string Icon { set; get; }
        public ActionExecutionType ExecutionType { set; get; }
        private DynamicObj _attr;
        public DynamicObj Attributes { get => _attr; }

        public ActionInfo(string rawUrl, RequestQueryString qs, string id)
        {
            _attr = new DynamicObj();
            this.RawUrl = rawUrl;
            this.Query = qs;
            this.ActionId = id;
            this.Title = id;

            this.Url = this.ToURL();
        }

        public string ToURL()
        {
            var str = this.RawUrl;

            if (this.Query != null)
            {
                if (str.Contains("?"))
                {
                    str += $"{this.Query.ToQueryString()}";
                }
                else
                {
                    str += $"?{this.Query.ToQueryString()}";
                }
            }

            return str;
        }
    }
}