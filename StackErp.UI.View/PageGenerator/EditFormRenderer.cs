using System;
using StackErp.Model;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View.PageGenerator
{
    public class EditFormRenderer : BasePageRenderer
    {
        public EditFormRenderer(EditFormContext context) : base(context)
        {
            FieldCompiler = new LayoutFieldCompiler(context);
        }

        protected override void Compile(LayoutContext layoutContext)
        {
            base.Compile(layoutContext);
            this.BuildDependencies();
            BuildActions();
        }
        protected void BuildDependencies()
        {

        }
        protected override void OnRenderComplete()
        {
            this.FillWidgetsData();            
            
            base.OnRenderComplete();
        }
        private void BuildActions()
        {
            var actionContext = new ActionContext(FormContext, ActionType.Save, "BTN_SAVE");
            actionContext.Query = FormContext.RequestQuery.Clone();
            FormContext.Actions.Add(PageActionCreator.Create(actionContext));
        }
        public void FillWidgetsData()
        {
            var recordModel = this.FormContext.EntityModel;
            foreach(var widgetKey in this.FormContext.Widgets)
            {
                var widget = widgetKey.Value;
                var fieldAttr = widget.Context.ControlDefinition.FieldAttribute;

                var value = recordModel.GetValue(fieldAttr.ValueField);
                widget.SetValue(value);
            }
        }

        public override ViewPage GetViewPage()
        {
            var page = base.GetViewPage();
            page.PageType = AppPageType.Edit;
            return page;
        }
    }
}
