using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StackErp.Model;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class BaseWidget
    {
        [JsonIgnore]
        public WidgetContext Context { get; }
        public string Caption { private set; get; }
        public virtual FormControlType WidgetType { get => FormControlType.None; }
        public string ControlId { private set; get; }
        public bool IsHidden { private set; get; }
        public object Value { private set; get; }
        public bool IsViewMode { private set; get; }
        private DynamicObj _props;
        public DynamicObj Properties { get => _props; }
        private Dictionary<string, IValidation> _validations;
        public Dictionary<string, IValidation> Validation { get => _validations; }
        public bool IsReadOnly { private set; get; }
        public bool IsRequired { private set; get; }
        public string DataUrl { private set; get; }
        //      RuleToFire: Array<{Index: number}>
        // Dependency: { Parents: Array<{Id: string}>, Children: Array<{Id: string}> }
        // FieldChangeSource: { SourceUrl: LinkInfo, DependUpon: Array<{Id: string}> }
        public bool IsEditable { private set; get; }
        protected object PostValue { private set; get; }

        public BaseWidget(WidgetContext context)
        {
            Context = context;
            _props = new DynamicObj();
            _validations = new Dictionary<string, IValidation>();
            this.Init(context);
        }

        public void Init(WidgetContext context) {
            this.Caption = context.Caption;
            this.ControlId = context.ControlId;
            
            //this.IsHidden = context.UIParams ? !!controlContext.UIParams.IsHidden : false;
            this.IsViewMode = context.FormContext.IsViewMode;
            this.PostValue = context.PostValue;
        }
        public virtual void OnCompile()
        {
            this.buildValidation();
        }
        public virtual bool OnSetData(object value)
        {
            return true;
        }

        public virtual void SetValue(object value)
        {
            if (this.OnSetData(value))
            {
                this.Value = value;
            }
        }

        public virtual object GetValue()
        {
            Value = PostValue;
            
            return this.Value;
        }

        public virtual void buildValidation()
        {
            
        }

        public void AddValidation(string key, IValidation validation)
        {
            if (!_validations.Keys.Contains(key))
                _validations.Add(key, validation);
        }
    }
}
