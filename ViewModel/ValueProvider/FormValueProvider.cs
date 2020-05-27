using System;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Form;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.ValueProvider
{
    public class FormValueProvider
    {
        private EditFormContext _formContext;
        public FormValueProvider(EditFormContext context)
        {
            _formContext = context;
        }

        public void ResolveFieldValue(UIFormField formField, Action<object, BaseField> onSetValue) 
        {
            var fieldSchema = this._formContext.GetField(formField.WidgetId);
            object value;
            ResolveData(fieldSchema, formField, out value);

            onSetValue.Invoke(value, fieldSchema);
        }

        public bool ResolveFieldValue(UIFormField formField, out object value) 
        {
               var fieldSchema = this._formContext.GetField(formField.WidgetId);

               return ResolveData(fieldSchema, formField, out value);
        }

        private bool ResolveData(BaseField fieldSchema, UIFormField formField, out object value)
        {
                var widget = BuildWidget(fieldSchema, formField);
                if (widget != null)
                {
                    this._formContext.AddControl(widget);

                    value = GetValue(widget, formField, fieldSchema);
                } 
                else
                    value = null;

                return true;
        }

        private BaseWidget BuildWidget(BaseField field, UIFormField fieldValue)
        {   
            var widgetContext = new WidgetContext(this._formContext);
            if (field == null) {
                widgetContext.Build(null, new StackErp.Model.Layout.TField() { FieldId = fieldValue.WidgetId });
            } else
                widgetContext.Build(field, null);
                
            widgetContext.WidgetType = (FormControlType)fieldValue.WidgetType;
            widgetContext.PostValue = fieldValue.Value;

            var widget = WidgetFactory.Create(widgetContext);
            return widget;
        }

        private object GetValue(BaseWidget widget, UIFormField fieldValue, BaseField fieldSchema)
        {
            //if CustomValueBinder exists then call it else
            var value = widget.GetValue();

            return value;
        }  
    }
}