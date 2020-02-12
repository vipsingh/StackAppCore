using System;
using StackErp.Model;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View.PageGenerator
{
    public class DetailFormRenderer : BasePageRenderer
    {
        public DetailFormRenderer(DetailFormContext context) : base(context)
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
