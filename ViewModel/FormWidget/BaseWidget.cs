using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class BaseWidget
    {
        public WidgetContext Context { get; }
        public string Name { get; }
        public string Caption { private set; get; }
        public FormControlType WidgetType { private set; get; }
        public string ControlId { private set; get; }
        public bool IsHidden { private set; get; }
        public object Value { private set; get; }
        public bool IsViewMode { private set; get; }
        private DynamicObj _props;
        public DynamicObj Properties { get => _props; }
        public string Validation { private set; get; }
        public bool IsReadOnly { private set; get; }
        public bool IsRequired { private set; get; }
        public string DataUrl { private set; get; }
        //      RuleToFire: Array<{Index: number}>
        // Dependency: { Parents: Array<{Id: string}>, Children: Array<{Id: string}> }
        // FieldChangeSource: { SourceUrl: LinkInfo, DependUpon: Array<{Id: string}> }
        public bool IsEditable { private set; get; }


        public BaseWidget(WidgetContext context)
        {
            Context = context;
            _props = new DynamicObj();
            this.Init(context);
        }

        public void Init(WidgetContext context) {
            this.Caption = context.Caption;
            this.ControlId = context.ControlId;
            this.WidgetType = context.WidgetType;
            //this.IsHidden = context.UIParams ? !!controlContext.UIParams.IsHidden : false;
            this.IsViewMode = context.FormContext.IsViewMode;
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
            if (this.Value != null)
            {
                return this.Value;
            }

            return null;
        }

        public virtual void buildValidation()
        {

        }
    }
}
