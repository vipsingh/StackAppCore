using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StackErp.Model;
using StackErp.Model.Form;
using StackErp.ViewModel.Helper;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class BaseWidget: IWidget
    {
        [JsonIgnore]
        public WidgetContext Context { get; }
        public string Caption { private set; get; }
        public virtual FormControlType WidgetType { get => FormControlType.None; }
        public string WidgetId { private set; get; }
        public bool IsHidden { set; get; }
        public object Value { protected set; get; }
        public DynamicObj AdditionalValue { protected set; get; }
        public string FormatedValue { protected set; get; }
        [JsonIgnore]
        public TypeCode BaseType { protected set; get; }
        public string WidgetFormatInfo { protected set; get; }
        public bool IsViewMode { private set; get; }
        private DynamicObj _props;
        public DynamicObj Properties { get => _props; }
        private InvariantDictionary<IValidation> _validations;
        public InvariantDictionary<IValidation> Validation { get => _validations; }
        public bool IsReadOnly { private set; get; }
        public bool IsRequired { private set; get; }
        public ActionInfo DataActionLink { protected set; get; }
        public List<int> RuleToFire  { protected set; get; }
        public WidgetDependencyInfo Dependency { set; get; }
        public WidgetFeatures Features { set; get; }
        // FieldChangeSource: { SourceUrl: LinkInfo, DependUpon: Array<{Id: string}> }        
        protected object PostValue { private set; get; }

        public BaseWidget(WidgetContext context)
        {
            Context = context;
            _props = new DynamicObj();            
            this.Init(context);
        }

        public void Init(WidgetContext context) {
            this.Caption = context.Caption;
            this.WidgetId = context.ControlId;
            
            //this.IsHidden = context.UIParams ? !!controlContext.UIParams.IsHidden : false;
            this.IsViewMode = context.FormContext.IsViewMode;
            this.PostValue = context.PostValue;
            this.IsReadOnly = context.IsReadOnly;
            this.IsRequired = context.IsRequired;
            this.BaseType = context.BaseType;
        }
        private bool _isCompiled = false;
        public virtual void OnCompile()
        {
            if (_isCompiled) return;
            this.BuildValidation();
            _isCompiled = true;
        }
        protected virtual bool OnSetData(object value)
        {
            this.Value = value;
            return true;
        }
        protected virtual bool OnFormatSetData(object value)
        {
            if (value == null)
            {
                FormatedValue = this.Context.AppContext.NotSpecifiedText;
                return false;
            }

            if (value is SelectOption)
            {
                FormatedValue = ((SelectOption)value).Text;
            }
            else if (value is ICollection<SelectOption>)
            {
                FormatedValue = String.Join(",", ((List<SelectOption>)value).Select(x => x.Text));
            }
            else
                FormatedValue = value.ToString();

            Value = value;

            return true;
        }

        public virtual bool SetValue(object value)
        {
            if (this.Context.IsViewMode) {
                return this.OnFormatSetData(value);
            } else
            {
                return this.OnSetData(value);
            }
        }

        public virtual object GetValue()
        {
            Value = PostValue;
            
            return this.Value;
        }

        public void SetAdditionalValue(string key, object value)
        {
            if (this.AdditionalValue == null)
                this.AdditionalValue = new DynamicObj();
            
            this.AdditionalValue.Add(key, value, true);
        }

        public virtual void ClearValue()
        {
            this.Value = null;
            this.FormatedValue = null;
            this.AdditionalValue = null;
        }

        #region Build Validation

        public virtual void BuildValidation()
        {
            if (this.IsRequired) 
            {
                AddValidation(ValidationConstant.Required, ValidationHelper.GetRequiredValidation(Caption));
            }

            var fieldValidation = this.Context.Validation;            
            if(fieldValidation != null)
            {
                if(fieldValidation.IsMendatory)
                {
                    AddValidation(ValidationConstant.Required, ValidationHelper.GetRequiredValidation(Caption));
                }
            }
            
        }

        public void AddValidation(string key, IValidation validation)
        {
            if (_validations == null)
                _validations = new InvariantDictionary<IValidation>();
                
            if (!_validations.Keys.Contains(key))
                _validations.AddItem(key, validation, true);
        }
        #endregion

        public void SetRule(int ruleId)
        {
            if(this.RuleToFire == null)
                this.RuleToFire = new List<int>();
                
            if(!this.RuleToFire.Contains(ruleId))
                this.RuleToFire.Add(ruleId);
        }

        protected virtual void BuildFormatInfo()
        {
            //WidgetFormatInfo
        }

        public virtual void OnCompileComplete(FormContext formContext)
        {

        }
    }

    public class WidgetFormatInfo
    {
        public string FormatString { set; get; }
        public string ColorCode { set; get; }
        public string FontSize {set;get;}
        public string FontWeight {set;get;}
    }

    public class ValueWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.None; }
        public ValueWidget(WidgetContext cntxt): base(cntxt)
        {

        }

        public override void OnCompile()
        {
            
        }
    }

    public class WidgetFeatures
    {
        public WidgetFeature Eval {set;get;}
        public WidgetFeature Invisible {set;get;}
        public WidgetFeature Mandatory {set;get;}
        public List<WidgetFeature> Options {set;get;}
    }

    public class WidgetFeature: DynamicObj
    {
        public FormFilterExpression Criteria { 
            set {
                this.Add("Criteria", value, true);
            } 
            get
            {
                return this.Get<FormFilterExpression>("Criteria", null);
            }
        }

        public List<string> Depends { 
            set {
                this.Add("Depends", value, true);
            } 
            get
            {
                return this.Get<List<string>>("Depends", null);
            }
        }
        
    }
}
