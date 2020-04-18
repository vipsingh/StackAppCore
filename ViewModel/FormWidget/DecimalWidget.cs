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
        }

        // protected override bool OnSetData(object value)
        // {
            
            
        //     return false;
        // }

        protected override bool OnFormatSetData(object value)
        {
            var formatter = new Core.Entity.EntityDataFormatter(this.Context.AppContext);
            var val =  formatter.FormatNumeric(value);

            return base.OnFormatSetData(val);
        }
    }
}
