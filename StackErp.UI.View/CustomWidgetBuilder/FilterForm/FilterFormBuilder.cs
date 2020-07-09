using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using StackErp.Core.Form;
using StackErp.Core.Layout;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Form;
using StackErp.Model.Layout;
using StackErp.UI.View.PageGenerator;
using StackErp.ViewModel;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.Model.ViewResponse;
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View.CustomWidgetBuilder
{    
    public class FilterFormBuilder
    {     
        StackAppContext Context;
        //protected LayoutFieldCompiler FieldCompiler {set; get;}
        public FilterFormBuilder(StackAppContext appContext)
        {                     
            Context = appContext;
            //FormContext = formContext;
            //FieldCompiler = new LayoutFieldCompiler(formContext);
        }

        public FilterWidgetResponse Generate(CustomRequestInfo request, RequestQueryString requestQuery)
        {
            var formContext = new EditFormContext(Context, requestQuery.EntityId, requestQuery);
            formContext.Build();   
            if (request != null && request.Value != null) 
            {
                JObject fieldId = (JObject)request.Value;
                var fid = fieldId["Value"];
                var fieldInfo = formContext.Entity.GetFieldSchema(Convert.ToInt32(fid));
                if (fieldInfo == null) {
                    throw new EntityException("Invalid field!");
                }

                CreateWidgets(formContext, fieldInfo);
                
            }

            var page = new ViewPage(formContext);

            var filtersRes = new List<ViewPage>(){ page };
            
            return new FilterWidgetResponse() { Filters = filtersRes };
        }

        public FilterWidgetResponse BuildWithData(CustomRequestInfo request, RequestQueryString requestQuery) 
        {
            if (request != null && request.Value != null) 
            {
                var filtersRes = new List<ViewPage>();
                var filterJson = request.Value;
                var exp = FilterExpression.BuildFromJson(requestQuery.EntityId, filterJson.ToString());
                foreach(var field in exp.GetAll()) 
                {
                    var formContext = new EditFormContext(Context, EntityCode.User, requestQuery);
                    formContext.Build();   

                    var fieldInfo = formContext.Entity.GetFieldSchema(field.FieldName);
                    CreateWidgets(formContext, fieldInfo, field);

                    var page = new ViewPage(formContext);
                    filtersRes.Add(page);
                }
            
                return new FilterWidgetResponse() { Filters = filtersRes };
            }

            return null;
        }

        private void CreateWidgets(FormContext formContext, BaseField fieldInfo, FilterExpField filterField = null) 
        {
            formContext.AddEntityModelInfo("FieldName", fieldInfo.Name);                

                formContext.AddControl(AddFieldName(formContext, fieldInfo));

                var widgetContext = new WidgetContext(formContext);            
                widgetContext.Build(fieldInfo, new TField() { FieldId = "Field" });
                widgetContext.IsRequired = false;
                widgetContext.Validation = null;

                var widget = WidgetFactory.Create(widgetContext);
                widget.OnCompile();
                if (filterField != null) {
                    widget.SetValue(filterField.Value);
                }
                formContext.AddControl(widget);

                AddOperationField(formContext, fieldInfo, filterField);
        }

        private void AddOperationField(FormContext formContext,BaseField fieldInfo, FilterExpField filterField = null) 
        {
            var widgetContext = WidgetContext.BuildContext(formContext, "OP"); 
            widgetContext.WidgetType = FormControlType.Dropdown;

            DropdownWidget widget = (DropdownWidget)ViewModel.FormWidget.WidgetFactory.Create(widgetContext);
            widget.Options = GetOptions(fieldInfo.BaseType, fieldInfo.Type);
            widget.OnCompile();
            if (filterField != null) {
                widget.SetValue((int)filterField.Op);
            }

            formContext.AddControl(widget);
        }

        private List<SelectOption> GetOptions(TypeCode type, FieldType fieldType) 
        {
            var ops = new List<SelectOption>();
            var opList = new List<FilterOperationType>();
            opList.Add(FilterOperationType.Equal);
            opList.Add(FilterOperationType.NotEqual);

            if (fieldType == FieldType.ObjectLink) 
            {
                opList.Add(FilterOperationType.In);
            }
            else if (type == TypeCode.Int32 || type == TypeCode.Decimal)
            {
                opList.Add(FilterOperationType.GreaterThan);
            }

            foreach(var l in opList)
            {
                var sel = new SelectOption();
                sel.Add(ViewConstant.Value, (int)l);
                sel.Add(ViewConstant.Text, l.ToString());
                ops.Add(sel);
            }

            return ops;
        }

        private IWidget AddFieldName(FormContext formContext, BaseField fieldInfo)
        {
            var widgetContext = WidgetContext.BuildContext(formContext, "Name"); 
            var label = new LabelWidget(widgetContext);
            label.SetValue(fieldInfo.Text);

            return label;
        }
    }
}