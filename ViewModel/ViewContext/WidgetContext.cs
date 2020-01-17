using System;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.ViewModel.ViewContext
{
    public class WidgetContext
    {
        public string Caption {private set;get;}
        public string ControlId {private set;get;}
        public FormControlType WidgetType {set;get;}
        public string UIParams {private set;get;}
        public BaseField FieldSchema {private set;get;}
        public FormContext FormContext {get;}
        public ControlDefinition ControlDefinition {private set;get;}
        public bool IsViewMode {get;}
        public IFieldValidation Validation {private set;get;}
        public bool IsReadOnly { private set; get; }
        public bool IsRequired { private set; get; }
        private DynamicObj _parameters;
        public DynamicObj Parameters
        {
            get { return _parameters; }
        }
        public object PostValue {set;get;}

        public WidgetContext(FormContext formContext) 
        {
            _parameters = new DynamicObj();
            FormContext = formContext;
            IsViewMode = formContext.IsViewMode;
        }

        public void Build(BaseField field)
        {
            ControlId = field.ViewName;
            Caption = field.Text;
            WidgetType = field.ControlType;
            Validation = field.Validations;
            IsRequired = field.IsRequired;
        }
    }
}
