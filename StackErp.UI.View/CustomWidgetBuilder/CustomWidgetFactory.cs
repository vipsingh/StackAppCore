using System;
using System.Collections;
using System.Collections.Generic;
using StackErp.Core.Form;
using StackErp.Core.Layout;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Form;
using StackErp.Model.Layout;
using StackErp.UI.View.CustomWidgetBuilder;
using StackErp.UI.View.PageGenerator;
using StackErp.ViewModel;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.Model.ViewResponse;
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View
{    
    public class CustomWidgetFactory
    {     
        private static Dictionary<FormControlType, Func<WidgetContext, TField, IWidget>> RegisteredWidgets;
        static CustomWidgetFactory()
        {
            RegisteredWidgets = new Dictionary<FormControlType, Func<WidgetContext, TField, IWidget>>();
            Add(FormControlType.ListForm, new ListFormWidgetBuilder().Build);
        }

        private static void Add(FormControlType type, Func<WidgetContext, TField, IWidget> func)
        {
            if (RegisteredWidgets.ContainsKey(type))
                throw new Exception("Control of type " + type + " is already registered.");

            RegisteredWidgets.Add(type, func);
        }

        public static bool HasKey(FormControlType widgetType)
        {
            return RegisteredWidgets.ContainsKey(widgetType);
        }

        public static Func<WidgetContext, TField, IWidget> Get(FormControlType widgetType)
        {
            return RegisteredWidgets[widgetType];
        }
    }
}