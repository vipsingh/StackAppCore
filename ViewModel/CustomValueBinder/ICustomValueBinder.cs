using System;
using StackErp.Model;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.CustomValueBinder
{
    public interface ICustomValueBinder
    {
        void SetValue(WidgetContext context, ref EntityModelBase record, UIFormField field);
        
        object GetValue(WidgetContext context, EntityModelBase record);
    }
}
