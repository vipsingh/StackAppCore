using System;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Layout;

namespace StackErp.ViewModel.ViewContext
{
    public class WidgetContext
    {
        public StackAppContext AppContext {private set;get;}
        public string Caption {set;get;}
        public string ControlId {set;get;}
        public FormControlType WidgetType {set;get;}
        public string UIParams {set;get;}
        public BaseField FieldSchema {private set;get;}
        public FormContext FormContext {get;}
        public ControlDefinition ControlDefinition {set;get;}
        public bool IsViewMode {get;}
        public TypeCode BaseType { protected set; get; }
        public IFieldValidation Validation {set;get;}
        public bool IsReadOnly { set; get; }
        public bool IsRequired { set; get; }
        public FormatInfo FormatInfo { set; get; }
        public TField LayoutField { set; get; }
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

        public void Build(BaseField field, TField layoutField)
        {
            if (field != null) 
            {   
                FieldSchema = field;
                ControlId = field.ViewName;
                Caption = field.Text;
                BaseType = field.BaseType;
                WidgetType = field.ControlType;
                Validation = field.Validations;
                IsRequired = field.IsRequired;
                ControlDefinition = field.ControlInfo;
                FormatInfo = field.FormatInfo;
                IsReadOnly = field.IsReadOnly;
            }

            if (layoutField != null)
            {   
                this.LayoutField = layoutField;

                if (!String.IsNullOrEmpty(layoutField.FieldId))
                    ControlId = layoutField.FieldId;
                if (!String.IsNullOrEmpty(layoutField.Text))
                    Caption = layoutField.Text;
                if (WidgetType == FormControlType.None)
                    WidgetType = layoutField.Widget;
            }
        }

        public void BuildCodeWidget() {
            //code component
        }

        public bool IsVisibile(Services.Evaluator Evaluator)
        {
            if (ControlDefinition != null)
            {
                var visibility = ControlDefinition.Visibility;

                if (visibility != null)
                    return Evaluator.EvaluateCondition(FormContext, visibility /* .Expression */);
            }

            return true;
        }

        public static WidgetContext BuildContext(FormContext formContext, string widgetId, ControlDefinition controlDef = null) 
        {
            var cnxt = new WidgetContext(formContext);
            cnxt.ControlId = widgetId;
            cnxt.ControlDefinition = controlDef;

            return cnxt;
        }

        public void AddParameter(string key, object value)
        {
            this._parameters.Add(key, value, true);
        }

        public T GetParameter<T>(string key, T def)
        {
            return this._parameters.Get(key, def);
        }
    }
}
