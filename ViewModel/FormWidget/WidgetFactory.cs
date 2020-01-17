using System;
using System.Collections.Generic;
using System.Linq;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public class WidgetFactory
    {
        static Dictionary<int, Func<WidgetContext, BaseWidget>> _widgets;
        static WidgetFactory()
        {
            _widgets = new Dictionary<int, Func<WidgetContext, BaseWidget>>();
            Add(FormControlType.TextBox, GetText);
            Add(FormControlType.DatePicker, GetDatePicker);
            Add(FormControlType.DecimalBox, GetDecimal);
            Add(FormControlType.Dropdown, GetDropdown);
            Add(FormControlType.EntityPicker, GetObjectPicker);
            Add(FormControlType.NumberBox, GetNumber);
            Add(FormControlType.CheckBox, GetCheckBox);

            Add(FormControlType.LongText, GetText);            
        }

        public static BaseWidget Create(WidgetContext cntxt)
        {
            if (!_widgets.Keys.Contains((int)cntxt.WidgetType))
                return null;

            return _widgets[(int)cntxt.WidgetType].Invoke(cntxt);
        }

        public static void Add(FormControlType type, Func<WidgetContext, BaseWidget> widget)
        {
            Add((int)type, widget);
        }

        public static void Add(int type, Func<WidgetContext, BaseWidget> widget)
        {
            if (!_widgets.Keys.Contains(type))
                _widgets.Add(type, widget);
        }

        private static BaseWidget GetText(WidgetContext cntxt)
        {
            return new TextWidget(cntxt);
        }

        private static BaseWidget GetDropdown(WidgetContext cntxt)
        {
            return new DropdownWidget(cntxt);
        }

        private static BaseWidget GetDatePicker(WidgetContext cntxt)
        {
            return new DatePickerWidget(cntxt);
        }

        private static BaseWidget GetDecimal(WidgetContext cntxt)
        {
            return new DecimalWidget(cntxt);
        }

        private static BaseWidget GetNumber(WidgetContext cntxt)
        {
            return new NumberWidget(cntxt);
        }
        private static BaseWidget GetObjectPicker(WidgetContext cntxt)
        {
            return new ObjectPickerWidget(cntxt);
        }
        private static BaseWidget GetCheckBox(WidgetContext cntxt)
        {
            return new CheckBoxWidget(cntxt);
        }
    }
}
