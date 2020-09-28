using System;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Form;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ValueProvider;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel
{
    public class EntityModelGenerator
    {
        private EditFormContext _formContext;
        public AnyStatus Status;

        public EntityModelBase RecordModel {private set; get;}
        private FormValueProvider ValueProvider;
        public EntityModelGenerator(EditFormContext context)
        {
            _formContext = context;
            Status = AnyStatus.NotInitialized;
            ValueProvider = new FormValueProvider(context);
        }
        public EntityModelBase Generate(UIFormModel model)
        {
            this.ValidatrRequest();

            _formContext.CreateDataModel();

            RecordModel = _formContext.EntityModel;

            SetRelationShip();

            this.SetFieldsModelToRecord(model);
            Status = AnyStatus.Success;

            return RecordModel;
        }

        public void SetFieldsModelToRecord(UIFormModel model)
        {
            foreach(var modelField in model.Widgets)
            {
                ValueProvider.ResolveFieldValue(modelField.Value, SetModelValue);
            }
        }        

        private void SetModelValue(object value, BaseField fieldSchema)
        {
            if (fieldSchema != null && RecordModel != null) {
                RecordModel.SetValue(fieldSchema.Name, value);
            }
        }     

        private bool IsVaildField(UIFormField formFieldsValue)
        {
            return true;
        }

        protected void ValidatrRequest()
        {

        }

        private void SetRelationShip()
        {
            if (_formContext.RequestQuery.RelatedEntityId.Code > 0 && _formContext.RequestQuery.RelatedObjectId > 0)
            {
                RecordModel.SetRelationValue(_formContext.RequestQuery.RelationField, _formContext.RequestQuery.RelatedEntityId, _formContext.RequestQuery.RelatedObjectId);
            }
        }
    }
}
