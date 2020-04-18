using System;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Form;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel
{
    public class EntityModelGenerator
    {
        private EditFormContext _formContext;
        public AnyStatus Status;

        public EntityModelBase RecordModel {private set; get;}
        public EntityModelGenerator(EditFormContext context)
        {
            _formContext = context;
            Status = AnyStatus.NotInitialized;
        }
        public EntityModelBase Generate(UIFormModel model)
        {
            this.ValidatrRequest();

            _formContext.CreateDataModel();

            RecordModel = _formContext.EntityModel;

            this.SetFieldsModelToRecord(model);
            Status = AnyStatus.Success;

            return RecordModel;
        }

        public void SetFieldsModelToRecord(UIFormModel model)
        {
            foreach(var modelField in model.Widgets)
            {
                var field = modelField.Value;
                var fieldSchema = this._formContext.GetField(field.WidgetId);
                var widget = BuildWidget(fieldSchema, field);
                if (widget != null)
                {
                    this._formContext.AddControl(widget);

                    SetValue(widget, field, fieldSchema);
                }
            }
        }

        public BaseWidget BuildWidget(BaseField field, UIFormField fieldValue)
        {   
            var widgetContext = new WidgetContext(this._formContext);
            widgetContext.Build(field, null);
            widgetContext.WidgetType = (FormControlType)fieldValue.WidgetType;
            widgetContext.PostValue = fieldValue.Value;

            var widget = WidgetFactory.Create(widgetContext);
            return widget;
        }

        protected void SetValue(BaseWidget widget, UIFormField fieldValue, BaseField fieldSchema)
        {
            //if CustomValueBinder exists then call it else
            var value = widget.GetValue();

            SetModelValue(fieldSchema, value);
        }   

        private  void SetModelValue(BaseField fieldSchema, object value)
        {
            RecordModel.SetValue(fieldSchema.Name, value);
        }     

        private bool IsVaildField(UIFormField formFieldsValue)
        {
            return true;
        }

        protected void ValidatrRequest()
        {

        }
    }
}
