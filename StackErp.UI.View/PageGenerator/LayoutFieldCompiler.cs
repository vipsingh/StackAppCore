using System;
using StackErp.Core.Layout;
using StackErp.Model.Entity;
using StackErp.Model.Layout;
using StackErp.ViewModel.FormWidget;
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

        public virtual void Compile(BaseField field, TField LayoutField)
        {
            if(!this.FormContext.Widgets.ContainsKey(LayoutField.FieldId))
            {
                var widget = BuildWidget(field, LayoutField);
                this.FormContext.AddControl(widget);
            }
        }

        public BaseWidget BuildWidget(BaseField field, TField LayoutField)
        {   
            var widgetContext = new WidgetContext(this.FormContext);            
            widgetContext.Build(field, LayoutField);

            var widget = WidgetFactory.Create(widgetContext);
            widget.OnCompile();

            return widget;
        }
    }
}
