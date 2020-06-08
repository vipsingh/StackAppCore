using System;
using System.Collections;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Form;
using StackErp.Model.Layout;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.Model.ViewResponse;
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View.CustomWidgetBuilder
{
    public class ListFormWidgetBuilder
    {
        public IWidget Build(WidgetContext context, TField layoutField)
        {
            if (context.FieldSchema != null && context.FieldSchema is OneToManyField)
            {
                var field = (OneToManyField)context.FieldSchema;

                var widget = new ListFormWidget(context);
                widget.FormPage = BuildRefForm(context.FormContext.Context, field.RefObject);
                widget.RelatedField = field.Name;
                
                return widget;
            }
            return null;
        }

        private ViewPage BuildRefForm(StackAppContext appContext, EntityCode refEntity)
        {
            var q = new RequestQueryString();
            q.EntityId = refEntity;
            var context = new EditFormContext(appContext, refEntity, q);
            context.Build();

            var builder = new PageBuilder.EntityPageBuilder();
            var page = builder.CreateNewPage(context);

            return page;
        }
    }
}