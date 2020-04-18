using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using StackErp.Model;
using StackErp.ViewModel.Helper;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class DropdownWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.Dropdown; }
        public List<SelectOption> Options {protected set;get;}
        public DropdownWidget(WidgetContext cntxt): base(cntxt)
        {
        }

        public override void OnCompile()
        {
            BuildLookupData();
        }

        protected virtual void BuildLookupData()
        {
            if (!this.IsViewMode) {
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
                var data = LookupDataHelper.GetLookupData(this.Context, new List<int>() { (int)value });   
                if (data.Count > 0) 
                    val = data[0];            
            }

            return val;
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
