using System;
using System.Collections;
using System.Collections.Generic;
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
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View.CustomWidgetBuilder
{    
    public class FilterFormBuilder
    {
        protected FormContext FormContext {private set; get;}
        //protected LayoutFieldCompiler FieldCompiler {set; get;}
        public FilterFormBuilder(StackAppContext appContext, RequestQueryString requestQuery)
        {
            var formContext = new EditFormContext(appContext, EntityCode.User, requestQuery);
            formContext.Build();

            FormContext = formContext;
            //FieldCompiler = new LayoutFieldCompiler(formContext);
        }

        public ViewPage Generate(CustomRequestInfo request)
        {
            if (request != null && request.Value != null) 
            {
                var fieldName = request.Value.Value.ToString();
                var fieldInfo = FormContext.Entity.GetFieldSchema(fieldName);

                FormContext.AddControl(AddFieldName(fieldInfo));

                var widgetContext = new WidgetContext(this.FormContext);            
                widgetContext.Build(fieldInfo, new TField() { FieldId = "Field" });
                widgetContext.IsRequired = false;
                widgetContext.Validation = null;

                var widget = WidgetFactory.Create(widgetContext);
                widget.OnCompile();
                FormContext.AddControl(widget);

                AddOperationField(fieldInfo);
            }

            var page = new ViewPage(FormContext);
            
            return page;
        }

        private void AddOperationField(BaseField fieldInfo) 
        {
            var widgetContext = WidgetContext.BuildContext(FormContext, "OP"); 
            widgetContext.WidgetType = FormControlType.Dropdown;

            DropdownWidget widget = (DropdownWidget)ViewModel.FormWidget.WidgetFactory.Create(widgetContext);
            widget.Options = GetOptions(fieldInfo.BaseType, fieldInfo.Type);
            widget.OnCompile();

            FormContext.AddControl(widget);
        }

        private List<SelectOption> GetOptions(BaseTypeCode type, FieldType fieldType) 
        {
            var ops = new List<SelectOption>();
            var opList = new List<FilterOperationType>();
            opList.Add(FilterOperationType.Equal);
            opList.Add(FilterOperationType.NotEqual);

            if (fieldType == FieldType.ObjectLink) 
            {
                opList.Add(FilterOperationType.In);
            }
            else if (type == BaseTypeCode.Int32 || type == BaseTypeCode.Decimal)
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

        private IWidget AddFieldName(BaseField fieldInfo)
        {
            var widgetContext = WidgetContext.BuildContext(FormContext, "Name"); 
            var label = new LabelWidget(widgetContext);
            label.SetValue(fieldInfo.Text);

            return label;
        }
    }
}