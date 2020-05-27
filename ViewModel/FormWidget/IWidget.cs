using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StackErp.Model;
using StackErp.Model.Form;
using StackErp.ViewModel.Helper;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.FormWidget
{
    public interface IWidget: IWidgetData
    {
        string Caption { get; }
        FormControlType WidgetType { get; }
        string WidgetId { get; }
        bool IsHidden { get; }
        bool IsViewMode { get; }
        DynamicObj Properties { get; }
        bool IsReadOnly { get; }
        bool IsRequired {get; }
        string WidgetFormatInfo { get; }
        InvariantDictionary<IValidation> Validation { get; }
        ActionInfo DataActionLink { get; }
        List<int> RuleToFire  { get; }
        WidgetDependencyInfo Dependency { set; get; }
        WidgetFeatures Features { set; get; }

        void OnCompile();
        void OnCompileComplete(FormContext formContext);
        bool SetValue(object value);
    }

    public interface IWidgetData
    {
        object Value { get; }
        string FormatedValue { get; }
        DynamicObj AdditionalValue {  get; }
    }
}