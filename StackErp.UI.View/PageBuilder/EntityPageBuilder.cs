using System;
using StackErp.ViewModel.Model;
using StackErp.UI.View.PageGenerator;
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View.PageBuilder
{
    public class EntityPageBuilder
    {
        public ViewPage CreateNewPage(EditFormContext context)
        {
            context.CreateDataModel();
            var lConext = new LayoutContext(context.Context, context.RequestQuery.LayoutId, context.RequestQuery.EntityId);
            lConext.Build();
            context.LayoutContext = lConext;

            var renderer = new EditFormRenderer(context);
            renderer.Generate(lConext);

            return renderer.GetViewPage();
        }
        public ViewPage CreateEditPage(EditFormContext context)
        {
            context.CreateDataModel();
            var lConext = new LayoutContext(context.Context, context.RequestQuery.LayoutId, context.RequestQuery.EntityId);
            lConext.Build();
            context.LayoutContext = lConext;

            var renderer = new EditFormRenderer(context);
            renderer.Generate(lConext);

            return renderer.GetViewPage();
        }
        public ViewPage CreateDetailPage(DetailFormContext context)
        {
            context.CreateDataModel();
            var lConext = new LayoutContext(context.Context, context.RequestQuery.LayoutId, context.RequestQuery.EntityId);
            lConext.Build();
            context.LayoutContext = lConext;

            var renderer = new DetailFormRenderer(context);
            renderer.Generate(lConext);

            return renderer.GetViewPage();
        }
        public ViewPage CreateDeskPage(DeskPageContext context)
        {
            var lConext = new DeskPageLayoutContext(context.Context, context.RequestQuery.LayoutId, context.RequestQuery.EntityId);
            lConext.Build();
            context.LayoutContext = lConext;

            var renderer = new DeskPageRenderer(context);
            renderer.Generate(lConext);

            return renderer.GetViewPage();
        }
    }
}
