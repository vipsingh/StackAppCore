using System;
using StackErp.Model;
using StackErp.Model.Layout;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View.PageGenerator
{
    public class DeskPageRenderer : BasePageRenderer
    {
        public DeskPageRenderer(DeskPageContext context) : base(context)
        {
            
        }

        protected override void Compile(LayoutContext layoutContext)
        {
            base.Compile(layoutContext);            
        }
        protected override void CompileWidgets(TView view)
        {
            foreach(var field in view.GetAllFields())
            {
                this.FieldCompiler.Compile(null, field);                                
            }
        }
        protected override void OnRenderComplete()
        {           
            BuildActions();
            
            base.OnRenderComplete();
        }
        private void BuildActions()
        {
            var q = new RequestQueryString();
            q.EntityId = this.FormContext.Entity.EntityId;
            var newAction = PageActionCreator.Create(new ActionContext(this.FormContext, ActionType.New, "NEW") { Query = q } );
            this.FormContext.Actions.Add(newAction);
        }
        public override ViewPage GetViewPage()
        {
            var page = base.GetViewPage();
            page.PageType = AppPageType.Desk;
            return page;
        }
    }
}
