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
        }
        protected void BuildDependencies()
        {

        }
        protected override void OnRenderComplete()
        {
            this.FillWidgetsData();
            BuildActions();
            
            base.OnRenderComplete();
        }
        private void BuildActions()
        {
            var actionContext = new ActionContext(FormContext, ActionType.Save, "BTN_SAVE");
            actionContext.Query = FormContext.RequestQuery;//.clone();
            FormContext.Actions.Add(PageActionCreator.Create(actionContext));
        }
        public void FillWidgetsData()
        {

        }

        public override ViewPage GetViewPage()
        {
            var page = new ViewPage(this.FormContext);
            return page;
        }
    }
}
