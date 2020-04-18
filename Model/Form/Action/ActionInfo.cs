using System;
using Newtonsoft.Json;

namespace StackErp.Model.Form
{
    public class ActionInfo
    {
        public string ActionId { set; get; }
        public string Title { set; get; }
        public string Url { get => this.ToURL(); }
        [JsonIgnore]
        public string RawUrl { set; get; }
        [JsonIgnore]
        public RequestQueryString Query { set; get; }
        public ActionType ActionType { set; get; }
        public ActionButtonPosition ActionPosition { set; get; }
        public ActionDisplayType DisplayType { set; get; }
        public string Icon { set; get; }
        public ActionExecutionType ExecutionType { set; get; }
        private DynamicObj _attr;
        public DynamicObj Attributes { get => _attr; }

        public ActionInfo(string rawUrl, RequestQueryString qs, string id): this(rawUrl, qs)
        {
            this.ActionId = id;
            this.Title = id;
        }
        public ActionInfo(string rawUrl, RequestQueryString qs)
        {
            _attr = new DynamicObj();
            this.RawUrl = rawUrl;
            this.Query = (qs == null) ? null : qs.Clone();
            ExecutionType = ActionExecutionType.Redirect;
        }

        public void AddAttribute(string key, object value)
        {
            this._attr.Add(key, value);
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