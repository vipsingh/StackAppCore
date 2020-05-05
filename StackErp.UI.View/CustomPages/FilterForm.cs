using System;
using StackErp.Core.Form;
using StackErp.Core.Layout;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Form;
using StackErp.Model.Layout;
using StackErp.UI.View.PageGenerator;
using StackErp.ViewModel;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View.CustomPages
{
    public class FilterForm: CustomPage
    {
        private StackAppContext _appContext;
        public FilterForm(StackAppContext appContext): base() 
        {
            _appContext = appContext;
        }

        public ViewPage GetPage() 
        {
            var formContext = new EditFormContext(_appContext, EntityCode.User, new RequestQueryString());
            formContext.Build();

            AddField(formContext);

            return this.View;
        }

        private void AddField(EditFormContext formContext) {
            var widgetContext = WidgetContext.BuildContext(formContext, "NameX"); 
            widgetContext.WidgetType = FormControlType.TextBox;

            var widget = ViewModel.FormWidget.WidgetFactory.Create(widgetContext);
            widget.OnCompile();

            AddFieldsInRow(new ViewModel.FormWidget.IWidget[]{ widget });
        }
    }
}
