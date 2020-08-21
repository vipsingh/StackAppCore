using System;
using StackErp.Model;
using StackErp.Model.Layout;
using StackErp.ViewModel.FormWidget;
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
            BuildActions();          
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
            this.FillWidgetsData();
            
            base.OnRenderComplete();
        }
        private void BuildActions()
        {
            var q = new RequestQueryString();
            q.EntityId = this.FormContext.Entity.EntityId;
            if (this.FormContext.Context.UserInfo.HasAccess(this.FormContext.Entity.EntityId, AccessType.Create))
            {
                var newAction = PageActionCreator.Create(new ActionContext(this.FormContext, ActionType.New, "NEW") { Query = q } );
                this.FormContext.Actions.Add(newAction);
            }
        }

        public void FillWidgetsData()
        {            
            foreach(var widgetKey in this.FormContext.Widgets)
            {
                var widget = widgetKey.Value;
                widget.SetValue(null);
            }
        }
        public override ViewPage GetViewPage()
        {
            var page = base.GetViewPage();
            page.PageType = AppPageType.Desk;
            return page;
        }
    }
}
