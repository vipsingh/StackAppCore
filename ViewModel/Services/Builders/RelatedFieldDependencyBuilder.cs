using System;
using System.Collections.Generic;
using System.Linq;
using StackErp.Model.DataList;
using StackErp.Model.Entity;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.Services
{
    public class RelatedFieldDependencyBuilder
    {
        public static void Build(FormContext context, BaseField field, IWidget widget)
        {
            if (field.IsRelatedField) 
            {
                var linkField = field.Related.LinkField;

                var ctrl = context.GetWidget(linkField);
                if(ctrl != null)
                {
                    if (ctrl.Dependency == null)
                        ctrl.Dependency = new WidgetDependencyInfo();

                    ctrl.Dependency.AddChild(new ChildDependencyInfo(){ WidgetId = widget.WidgetId });
                }
            }
        }
    }

}