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
    public class FilterDependencyBuilder
    {
        public static void Build(FormContext context, BaseField field, IWidget widget)
        {
            List<string> parentFileds = new List<string>();
            if (field.ControlInfo.DataSource != null)
            {
                var domain = field.ControlInfo.DataSource.Domain;
                if (domain != null)
                {
                    var filterFields = domain.GetAll().Where(x=> x.ValueType == FilterValueType.EntityField);
                    if (filterFields.Count() > 0)
                    {
                        parentFileds.AddRange(filterFields.Select(x => x.Value.ToString()));
                    }
                }
            }

            if (parentFileds.Count > 0)
            {
                if (widget.Dependency == null)
                    widget.Dependency = new WidgetDependencyInfo();

                widget.Dependency.Parents = BuildParents(context, parentFileds, field.Name);
            }
        }

        private static List<ParentDependencyInfo> BuildParents(FormContext context, List<string> parentFileds, string parentId)
        {
            var parents = new List<ParentDependencyInfo>();
            foreach(string s in parentFileds)
            {
                var p = new ParentDependencyInfo(){
                    WidgetId = s,
                    IsRequired = true
                };
                parents.Add(p);

                var childCtrl = context.GetWidget(s);
                if(childCtrl != null)
                {
                    if (childCtrl.Dependency == null)
                        childCtrl.Dependency = new WidgetDependencyInfo();

                    childCtrl.Dependency.AddChild(new ChildDependencyInfo(){ WidgetId = parentId, Clear = true });
                }

                context.MissingFields.Add(s);
            }

            return parents;
        }
    }
}