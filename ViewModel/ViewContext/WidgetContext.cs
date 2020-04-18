using System;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Layout;

namespace StackErp.ViewModel.ViewContext
{
    public class WidgetContext
    {
        public StackAppContext AppContext {private set;get;}
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
        public FormatInfo FormatInfo { private set; get; }
        private DynamicObj _parameters;
        public DynamicObj Parameters
        {
            get { return _parameters; }
        }
        public object PostValue {set;get;}

        public WidgetContext() 
        {
            _parameters = new DynamicObj();            
        }
        public WidgetContext(FormContext formContext) : base()
        {
            _parameters = new DynamicObj();
            FormContext = formContext;
            IsViewMode = formContext.IsViewMode;
            AppContext = formContext.Context;
        }

        public void Build(BaseField field, TField LayoutField)
        {
            if (LayoutField != null)
            {
                ControlId = LayoutField.FieldId;
                Caption = LayoutField.Text;
                WidgetType = LayoutField.Widget;
            }
            
            if (field != null) 
            {
                ControlId = field.ViewName;
                if (Caption == null)
                    Caption = field.Text;
                WidgetType = field.ControlType;
                Validation = field.Validations;
                IsRequired = field.IsRequired;
                ControlDefinition = field.ControlInfo;
                FormatInfo = field.FormatInfo;
                IsReadOnly = field.IsReadOnly;
            }
        }
    }
}
