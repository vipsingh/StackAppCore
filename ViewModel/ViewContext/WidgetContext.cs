using System;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.ViewModel.ViewContext
{
    public class WidgetContext
    {
        public string Caption {private set;get;}
        public string ControlId {private set;get;}
        public FormControlType WidgetType {private set;get;}
        public string UIParams {private set;get;}
        public BaseField FieldSchema {private set;get;}
        public FormContext FormContext {private set;get;}
        public ControlDefinition ControlDefinition {private set;get;}
        public bool IsViewMode {private set;get;}
        public IFieldValidation Validation {private set;get;}
        private DynamicObj _parameters;
        public DynamicObj Parameters
        {
            get { return _parameters; }
        }

        public WidgetContext() 
        {
            _parameters = new DynamicObj();
        }
    }
}
