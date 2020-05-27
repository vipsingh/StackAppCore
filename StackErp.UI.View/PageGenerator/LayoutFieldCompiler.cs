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
        protected FormContext FormContext {private set; get;}
        public LayoutFieldCompiler(FormContext context)
        {
            this.FormContext = context;
        }

        public virtual void Compile(BaseField field, TField LayoutField = null)
        {
            if (LayoutField == null || LayoutField.FieldId == null) {
                LayoutField = new TField(){ FieldId = field.Name };
            }
            if (LayoutField != null)
            {
                if (LayoutField.Widget == FormControlType.None && field != null)
                    LayoutField.Widget = field.ControlType;
            }
            
            if(!this.FormContext.Widgets.ContainsKey(LayoutField.FieldId))
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
            if(CustomWidgetFactory.HasKey(LayoutField.Widget))
            {
                    widget = CustomWidgetFactory.Get(LayoutField.Widget).Invoke(widgetContext, LayoutField);
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
                ProcessEntityField(field, widget);
            }
            
            return widget;
        }   

        private void ProcessEntityField(BaseField field, IWidget widget)
        {
            if (field.IsComputed)
            {
                PrepareComputedField(widget, field);
            }
        }     

        public void PrepareComputedField(IWidget widget, BaseField field)
        {
            var exp = field.ComputeExpression;
            foreach(var f in exp.KeyWords)
            {
                this.FormContext.AddMissingField(f);
            }
            
            var feture = new WidgetFeature();
            feture.Depends = exp.KeyWords;
            feture.Add(ViewConstant.Expression, exp);

            AddFeature(widget, feture, "EVAL");
        }

        private void AddFeature(IWidget widget, WidgetFeature feature, string type)
        {
            if (widget.Features == null)
                widget.Features = new WidgetFeatures();
            
            if (type == "EVAL")
                widget.Features.Eval = feature;
            else  if (type == "INVISIBLE")
                widget.Features.Invisible = feature;
            
        }
    }
}
