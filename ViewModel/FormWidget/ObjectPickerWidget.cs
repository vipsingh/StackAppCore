using System;
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
                this.SetAdditionalValue(ViewConstant.ViewLink, StackErp.Model.AppLinkProvider.GetDetailPageLink(this.Context.ControlDefinition.FieldAttribute.RefEntity, ((SelectOption)val).Value));
            }
            
            return base.OnFormatSetData(val);
        }
    }
}
