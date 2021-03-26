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
        public bool IsMultiSelect {private set; get;} 
        public ObjectPickerWidget(WidgetContext cntxt): base(cntxt)
        {
        }
        public override void OnCompile()
        {
            base.OnCompile();
            if (this.Context.ControlDefinition != null) 
            {
                IsMultiSelect = this.Context.ControlDefinition.IsMultiSelect;
            }

            if (!IsViewMode) 
            {
                this.DataActionLink = PreparePickerLink(this.Context);
            }
        }

        private ActionInfo PreparePickerLink(WidgetContext cntxt)
        {            
            var q = new RequestQueryString();
            q.WidgetId = this.WidgetId;
            q.EntityId = cntxt.FormContext.RequestQuery.EntityId;
            if (cntxt.FieldSchema != null) {
                q.FieldName = cntxt.FieldSchema.Name;
            }
            
            var link = new ActionInfo("widget/GetPickerData", q);
            link.ActionType = ActionType.Custom;
            link.ExecutionType = ActionExecutionType.Custom;

            return link;
        }

        protected override bool OnFormatSetData(object value)
        {
            object val = value;
            if (!this.IsMultiSelect)
            {
                if (value is SelectOption)
                {
                    this.SetAdditionalValue(ViewConstant.ViewLink, StackErp.Model.AppLinkProvider.GetDetailPageLink(this.Context.ControlDefinition.FieldAttribute.RefEntity, ((SelectOption)val).Value).Url);
                }
            }
            
            return base.OnFormatSetData(val);
        }

         public override object GetValue()
        {
            Value = DataHelper.ResolveWidgetValue(PostValue, this.BaseType);
            
            return this.Value;
        }
    }
}
