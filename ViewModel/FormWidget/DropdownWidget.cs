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
        public DropdownWidget(WidgetContext cntxt): base(cntxt)
        {
        }

        public override void OnCompile()
        {
            BuildLookupData();
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

        protected virtual void BuildLookupData()
        {
            if (!this.IsViewMode && this.Options == null) {
                var data = LookupDataHelper.GetLookupData(this.Context);
                this.Options = data;
            }
        }

        protected override bool OnSetData(object value)
        {
            this.Value = this.SetOption(value);
            return true;
        }

        private SelectOption SetOption(object value)
        {
            SelectOption val = null;
            if (value != null && value is int)
            {
                List<SelectOption> data;
                if (this.Options != null) {
                    data = this.Options.Where(o => o.Value == (int)value).ToList();
                }
                else
                    data = LookupDataHelper.GetLookupData(this.Context, new List<int>() { (int)value });   

                if (data.Count > 0) 
                    val = data[0];
                else 
                {
                    var op  = new SelectOption();
                    op.Add(ViewConstant.Text, value.ToString());
                    op.Add(ViewConstant.Value, value);
                    AddOption(op);
                    val = op;
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
            if(PostValue is JObject)
            {
                var v = (JObject)PostValue;
                Value = v["Value"].ToString();
            } else {
                Value = DataHelper.GetDataValue(PostValue, TypeCode.Int32);
            }
            
            return this.Value;
        }
    }
}
