using System;
using System.Collections.Generic;
using System.Linq;
using StackErp.Model.DataList;
using StackErp.Model.Entity;
using StackErp.Model.Layout;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.Services
{
    public class WidgetFeatureBuilder
    {
        public static void Build(WidgetContext context, IWidget widget, BaseField field, TField layoutField)
        {
            BuildInvisible(context, widget, field, layoutField);
            BuildReadonly(context, widget, field, layoutField);
        }

        private static void BuildInvisible(WidgetContext context, IWidget widget, BaseField field, TField layoutField)
        {
            if (layoutField == null || String.IsNullOrEmpty(layoutField.Invisible)) return;

            var exp = layoutField.Invisible;
            if (exp == "1") 
            {
                widget.IsHidden = true;
                return;
            }
            var formContext = context.FormContext;

            var criteria = FilterExpression.BuildFromJson(formContext.Entity.EntityId, exp);
            AddMissingFields(context.FormContext, criteria);

            var feture = new WidgetFeature();
            feture.Depends = criteria.GetFieldNames() as List<string>;
            feture.Criteria = new FormFilterExpression(formContext, criteria);

            AddFeature(widget, feture, "INVISIBLE");
        }

        private static void BuildReadonly(WidgetContext context, IWidget widget, BaseField field, TField layoutField)
        {
            if (layoutField == null || String.IsNullOrEmpty(layoutField.ReadOnly)) return;

            var exp = layoutField.ReadOnly;
            if (exp == "1") 
            {
                widget.IsReadOnly = true;
                return;
            }
            var formContext = context.FormContext;

            var criteria = FilterExpression.BuildFromJson(formContext.Entity.EntityId, exp);
            AddMissingFields(context.FormContext, criteria);

            var feture = new WidgetFeature();
            feture.Depends = criteria.GetFieldNames() as List<string>;
            feture.Criteria = new FormFilterExpression(formContext, criteria);

            AddFeature(widget, feture, "READONLY");
        }

        private static void AddMissingFields(FormContext context, FilterExpression exp)
        {
            foreach (var f in exp.GetFieldNames())
            {
                context.AddMissingField(f);
            }
        }

        public static void PrepareComputedField(WidgetContext context, IWidget widget, BaseField field)
        {
            var exp = field.ComputeExpression;
            foreach (var f in exp.KeyWords)
            {
                context.FormContext.AddMissingField(f);
            }

            var feture = new WidgetFeature();
            feture.Depends = exp.KeyWords;
            feture.Add(ViewConstant.Expression, exp);

            AddFeature(widget, feture, "EVAL");
        }

        private static void AddFeature(IWidget widget, WidgetFeature feature, string type)
        {
            if (widget.Features == null)
                widget.Features = new WidgetFeatures();

            if (type == "EVAL")
                widget.Features.Eval = feature;
            else if (type == "INVISIBLE")
                widget.Features.Invisible = feature;
            else if (type == "READONLY")
                widget.Features.ReadOnly = feature;

        }
    }

}