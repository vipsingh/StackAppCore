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
            var lConext = new LayoutContext(context);
            lConext.Build();

            var renderer = new EditFormRenderer(context);
            renderer.Generate(lConext);

            return renderer.GetViewPage();
        }
        public ViewPage CreateEditPage(EditFormContext context)
        {
            context.CreateDataModel();
            var lConext = new LayoutContext(context);
            lConext.Build();

            var renderer = new EditFormRenderer(context);
            renderer.Generate(lConext);

            return renderer.GetViewPage();
        }
    }
}
