using System;
using Newtonsoft.Json.Linq;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class DropdownWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.Dropdown; }
        public DropdownWidget(WidgetContext cntxt): base(cntxt)
        {
        }

        public override object GetValue()
        {
            if(PostValue is JObject)
            {
                var v = (JObject)PostValue;
                Value = v["Id"].ToString();
            } else {
                Value = DataHelper.GetDataValue(PostValue, TypeCode.Int32);
            }
            
            return this.Value;
        }
    }
}
