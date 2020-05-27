using System;
using Newtonsoft.Json.Linq;
using StackErp.Model;
using StackErp.Model.Form;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class ImageField: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.Image; }
        public ActionInfo UploadLink {protected set;get;}
        public ImageField(WidgetContext cntxt): base(cntxt)
        {
        }

         public override void OnCompile()
        {
            base.OnCompile();
            if (!IsViewMode) 
            {
                this.UploadLink = PrepareUploadLink();
            }
        }

        private ActionInfo PrepareUploadLink()
        {            
            var q = new RequestQueryString();
            q.WidgetId = this.WidgetId;
            q.EntityId = this.Context.FormContext.RequestQuery.EntityId;
            
            var link = new ActionInfo(Context.AppContext.AppRoot + "File/UploadImage", q);
            link.ActionType = ActionType.Custom;
            link.ExecutionType = ActionExecutionType.Custom;

            return link;
        }

        private string PrepareImageUrl(string fileName)
        {            
            var q = new RequestQueryString();
            q.Name = fileName;
            var link = new ActionInfo(Context.AppContext.AppRoot + "file/objectimage", q);
            link.ActionType = ActionType.Custom;
            link.ExecutionType = ActionExecutionType.Custom;

            return link.Url;
        }

        protected override bool OnFormatSetData(object value)
        {
            if (value == null) return true;

            var url =  PrepareImageUrl(value.ToString());

            return base.OnFormatSetData(url);
        }

        protected override bool OnSetData(object value)
        {
            if (value == null) return true;
            
            this.Value = new { Url = PrepareImageUrl(value.ToString()) };
            return true;
        }

        public override object GetValue()
        {
            if(PostValue is JObject)
            {
                var v = (JObject)PostValue;
                var fileName = v["FileName"].ToString();
                var isTemp = Convert.ToBoolean(v["IsTemp"].ToString());
                var obj = new DynamicObj();
                obj.Add("IsTemp", isTemp);
                obj.Add("FileName", fileName);

                return obj;
            }

            return null;
        }
    }
}
