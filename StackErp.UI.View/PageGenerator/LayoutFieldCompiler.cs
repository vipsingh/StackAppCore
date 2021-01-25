using System;
using StackErp.Core.Layout;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Layout;
using StackErp.ViewModel;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.Services;
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View.PageGenerator
{
    public class LayoutFieldCompiler
    {
        protected FormContext FormContext { private set; get; }
        public LayoutFieldCompiler(FormContext context)
        {
            this.FormContext = context;
        }

        public virtual void Compile(BaseField field, TField LayoutField = null)
        {
            if (LayoutField == null || LayoutField.FieldId == null)
            {
                LayoutField = new TField() { FieldId = field.Name };
            }
            if (LayoutField != null)
            {
                if (LayoutField.WidgetType == FormControlType.None && field != null)
                    LayoutField.WidgetType = field.ControlType;
            }

            if (!this.FormContext.Widgets.ContainsKey(LayoutField.FieldId))
            {
                IWidget widget = BuildWidget(field, LayoutField);

                this.FormContext.AddControl(widget);
            }
        }

        public IWidget BuildWidget(BaseField field, TField LayoutField)
        {
            var widgetContext = new WidgetContext(this.FormContext);
            widgetContext.Build(field, LayoutField);

            IWidget widget;
            if (CustomWidgetFactory.HasKey(LayoutField.WidgetType))
            {
                widget = CustomWidgetFactory.Get(LayoutField.WidgetType).Invoke(widgetContext, LayoutField);
            }
            else
            {
                if (field != null && field.IsComputed)
                    widgetContext.WidgetType = FormControlType.Label;

                widget = WidgetFactory.Create(widgetContext);
            }
            widget.OnCompile();

            if (field != null)
            {
                ProcessEntityField(widgetContext, field, widget, LayoutField);
            }

            WidgetFeatureBuilder.Build(widgetContext, widget, field, LayoutField);

            return widget;
        }

        private void ProcessEntityField(WidgetContext context, BaseField field, IWidget widget, TField LayoutField)
        {
            if (field.IsComputed)
            {
                WidgetFeatureBuilder.PrepareComputedField(context, widget, field);
            }
        }

    }
}
