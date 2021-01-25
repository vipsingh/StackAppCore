using System;
using StackErp.Core.Form;
using StackErp.Model;
using StackErp.Model.Layout;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View.PageGenerator
{
    public class DetailFormRenderer : BasePageRenderer
    {
        public DetailFormRenderer(DetailFormContext context) : base(context)
        {
            
        }

        protected override void Compile(LayoutContext layoutContext)
        {
            base.Compile(layoutContext);                    
        }

        protected override void OnRenderComplete()
        {
            this.FillWidgetsData();
            
            base.OnRenderComplete();
        }

        protected override void CompileActions(TView view)
        {
             if (this.FormContext.Context.UserInfo.HasAccess(FormContext.Entity.EntityId, AccessType.Update))
            {
                var actionContext = new ActionContext(FormContext, ActionType.Edit, "BTN_EDIT");
                actionContext.Query = FormContext.RequestQuery.Clone();
                FormContext.Actions.Add(PageActionCreator.Create(actionContext));
            }
            
            if(view.Commands != null) 
            {
                foreach(var command in view.Commands)
                {
                    var c = EntityActionService.GetViewAction(this.FormContext.Context, this.FormContext.Entity.EntityId, FormContext.EntityLayoutType, command.Id);
                    if(c!= null)
                    {
                        this.FormContext.Actions.Add(PageActionCreator.BuildActionFromDefinition(c, this.FormContext));
                    }
                }
            }
        }
        public void FillWidgetsData()
        {
            var recordModel = this.FormContext.EntityModel;
            foreach(var widgetKey in this.FormContext.Widgets)
            {
                var widget = widgetKey.Value;
                var fieldAttr = ((BaseWidget)widget).Context.ControlDefinition.FieldAttribute;

                var value = recordModel.GetValue(fieldAttr.ValueField);
                widget.SetValue(value);
            }
        }

        public void SetWidgetValue() 
        {
            
        }

        public override ViewPage GetViewPage()
        {
            var page = base.GetViewPage();
            page.PageType = AppPageType.Detail;
            return page;
        }
    }
}
