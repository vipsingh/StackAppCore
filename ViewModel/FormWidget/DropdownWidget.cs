using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using StackErp.Model;
using StackErp.ViewModel.Helper;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class DropdownWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.Dropdown; }       
        public bool IsMultiSelect {private set; get;} 
        public DropdownWidget(WidgetContext cntxt): base(cntxt)
        {
        }

        public override void OnCompile()
        {
            if (this.Context.ControlDefinition != null) 
            {
                IsMultiSelect = this.Context.ControlDefinition.IsMultiSelect;
            }
            BuildCollectionData();
        }

        List<SelectOption> _options = null;
        public List<SelectOption> Options {
            get
            {
                return _options;
            }
            set
            {
                _options = value;

                // if (!IsBuildOnDataCompleted && options != null && options.Count > 0)
                // {
                //     InitalizeFirstOption();
                // }
            }}

        protected virtual void BuildCollectionData()
        {
            if (!this.IsViewMode && this.Options == null) {
                var data = CollectionDataHelper.GetCollectionData(this.Context);
                this.Options = data;
            }
        }

        protected override bool OnSetData(object value)
        {
            this.Value = this.SetOption(value);
            return true;
        }

        private object SetOption(object value)
        {
            object val = null;
            if (value != null)
            {
                List<int> values;
                List<SelectOption> data;

                if (value is SelectOption)
                {
                    data = new List<SelectOption>() { value as SelectOption };
                }
                else if (value is List<SelectOption>)
                {
                    data = (List<SelectOption>)value;
                }
                else
                {    
                    if (value is int) {
                        values = new List<int>(){ (int) value };
                    } else {
                        values = (List<int>)value;
                    }
                    

                    if (this.Options != null) {
                        data = this.Options.Where(o => values.Contains(o.Value)).ToList();
                    }
                    else
                        data = CollectionDataHelper.GetCollectionData(this.Context, values);
                }

                if (data.Count > 0) {
                    val = data;
                    if (!IsMultiSelect) {
                        val = data[0];
                    }
                }
                else 
                {
                    if (!IsMultiSelect) {
                        var op  = new SelectOption();
                        op.Add(ViewConstant.Text, value.ToString());
                        op.Add(ViewConstant.Value, value);
                        AddOption(op);
                        val = op;
                    }
                }
            }

            return val;
        }

        private void AddOption(SelectOption option)
        {
            if (Options == null)
                Options = new List<SelectOption>();

            Options.Add(option);            
        }

        protected override bool OnFormatSetData(object value)
        {
            var val = this.SetOption(value);

            return base.OnFormatSetData(val);
        }

        public override object GetValue()
        {
            Value = DataHelper.ResolveWidgetValue(PostValue, this.BaseType);
            
            return this.Value;
        }
    }
}
