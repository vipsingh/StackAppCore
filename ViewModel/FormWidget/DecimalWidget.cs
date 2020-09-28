using System;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class DecimalWidget: BaseWidget
    {
        public override FormControlType WidgetType { get => FormControlType.DecimalBox; }
        public int DecimalPlaces {set;get;}
        public DecimalWidget(WidgetContext cntxt): base(cntxt)
        {
            DecimalPlaces = 2;
        }

        // protected override bool OnSetData(object value)
        // {
            
            
        //     return false;
        // }
    }
}
