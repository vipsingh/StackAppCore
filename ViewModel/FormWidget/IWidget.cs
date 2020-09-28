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
    public interface IWidget: IDisplayWidget
    {
        bool IsViewMode { get; }
        DynamicObj Properties { get; }
        bool IsReadOnly { set; get; }
        bool IsRequired {get; }
        InvariantDictionary<IValidation> Validation { get; }
        ActionInfo DataActionLink { get; }
        List<int> RuleToFire  { get; }
        WidgetDependencyInfo Dependency { set; get; }
        WidgetFeatures Features { set; get; }
        IWidgetData ToOnlyData();
    }

    public interface IDisplayWidget
    {
        string Caption { get; }
        FormControlType WidgetType { get; }
        string WidgetId { get; }
        bool IsHidden { set;get; }
        string FormatedValue { get; }
        DynamicObj AdditionalValue {  get; }
        object Value { get; }
        WidgetFormatInfo FormatInfo { get; }
        void ClearValue();

        void OnCompile();
        void OnCompileComplete(FormContext formContext);
        bool SetValue(object value);
        void SetAdditionalValue(string key, object value);
    }

    public interface IWidgetData
    {
        string FormatedValue { get; }
        DynamicObj AdditionalValue {  get; }
        object Value { get; }
        ActionInfo DataLink { get; }
    }
}