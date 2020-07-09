using System;
using StackErp.ViewModel.Model;
using StackErp.UI.View.PageGenerator;
using StackErp.ViewModel.ViewContext;
using StackErp.Model;

namespace StackErp.UI.View.PageBuilder
{
    public class EntityPageBuilder
    {
        public ViewPage CreateNewPage(EditFormContext context)
        {
            if (!context.Context.UserInfo.HasAccess(context.Entity.EntityId, AccessType.Create))
                throw new AuthException("Operation not allowed.");

            context.CreateDataModel();
            var lConext = new LayoutContext(context.Context, context.ItemTypeId, context.Entity.EntityId);
            lConext.Build();
            context.LayoutContext = lConext;

            var renderer = new EditFormRenderer(context);
            renderer.Generate(lConext);

            return renderer.GetViewPage();
        }
        public ViewPage CreateEditPage(EditFormContext context)
        {
            if (!context.Context.UserInfo.HasAccess(context.Entity.EntityId, AccessType.Update))
                throw new AuthException("Operation not allowed.");

            context.CreateDataModel();
            var lConext = new LayoutContext(context.Context, context.ItemTypeId, context.Entity.EntityId);
            lConext.Build();
            context.LayoutContext = lConext;

            var renderer = new EditFormRenderer(context);
            renderer.Generate(lConext);

            return renderer.GetViewPage();
        }
        public ViewPage CreateDetailPage(DetailFormContext context)
        {
            if (!context.Context.UserInfo.HasAccess(context.Entity.EntityId, AccessType.Read))
                throw new AuthException("Operation not allowed.");

            context.CreateDataModel();
            var lConext = new LayoutContext(context.Context, context.ItemTypeId, context.Entity.EntityId);
            lConext.Build();
            context.LayoutContext = lConext;

            var renderer = new DetailFormRenderer(context);
            renderer.Generate(lConext);

            return renderer.GetViewPage();
        }
        public ViewPage CreateDeskPage(DeskPageContext context)
        {
            if (!context.Context.UserInfo.HasAccess(context.Entity.EntityId, AccessType.Read))
                throw new AuthException("Operation not allowed.");
                
            var lConext = new DeskPageLayoutContext(context.Context, 0, context.RequestQuery.EntityId);
            lConext.Build();
            context.LayoutContext = lConext;

            var renderer = new DeskPageRenderer(context);
            renderer.Generate(lConext);

            return renderer.GetViewPage();
        }
    }
}
