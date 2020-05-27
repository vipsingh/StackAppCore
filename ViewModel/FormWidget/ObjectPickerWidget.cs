using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using StackErp.Model;
using StackErp.Model.Form;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class ObjectPickerWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.EntityPicker; }    
        public ObjectPickerWidget(WidgetContext cntxt): base(cntxt)
        {
        }
        public override void OnCompile()
        {
            base.OnCompile();
            if (!IsViewMode) 
            {
                this.DataActionLink = PreparePickerLink();
            }
        }

        private ActionInfo PreparePickerLink()
        {            
            var q = new RequestQueryString();
            q.WidgetId = this.WidgetId;
            q.EntityId = this.Context.FormContext.RequestQuery.EntityId;
            
            var link = new ActionInfo("widget/GetPickerData", q);
            link.ActionType = ActionType.Custom;
            link.ExecutionType = ActionExecutionType.Custom;

            return link;
        }

        protected override bool OnFormatSetData(object value)
        {
            object val = null;
            if (value is SelectOption)
            {
                val = value;
                this.SetAdditionalValue(ViewConstant.ViewLink, StackErp.Model.AppLinkProvider.GetDetailPageLink(this.Context.ControlDefinition.FieldAttribute.RefEntity, ((SelectOption)val).Value).Url);
            }
            
            return base.OnFormatSetData(val);
        }

         public override object GetValue()
        {
            if(PostValue is JObject)
            {
                var v = (JObject)PostValue;
                Value = v["Value"].ToString();
                
            } else if (PostValue is JArray) {
                var postVals = new List<int>();
                foreach(var o in (JArray)PostValue) {
                    if (o is JObject)
                        postVals.Add((int)DataHelper.GetDataValue(o["Value"], TypeCode.Int32));
                    else
                        postVals.Add((int)DataHelper.GetDataValue(o, TypeCode.Int32));
                }
                Value = postVals;
            }
            else {
                Value = DataHelper.GetDataValue(PostValue, TypeCode.Int32);
            }
            
            return this.Value;
        }
    }
}
