using System;
using StackErp.Core.DataList;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;
using StackErp.Model.Layout;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.DataList
{
    public class EntityListBuilder: DataListBuilder
    {
        protected override DataListDefinition GetSourceDefinition(DataListContext context)
        {
            EntityListService service = new EntityListService();
             
            return service.GetEntityListDefn(context.SourceEntityId);
        }

        protected override void Compile(DataListContext context, DataListDefinition defn)
        {
            //build header
            PrepareFields(context, defn);

        }        

        protected void PrepareFields(DataListContext context, DataListDefinition defn)
        {
            var formContext = new ViewContext.DetailFormContext(context.Context, context.SourceEntityId, context.Context.RequestQuery);
            
            AddField(context, formContext, new TField { FieldName = context.IdColumn });

            foreach(var tField in defn.Layout.Fields)
            {
                AddField(context, formContext, tField);
            }
        }

        protected void AddField(DataListContext context, FormContext formContext, TField tField)
        {
                BaseField field = null;
                if (!String.IsNullOrEmpty(tField.FieldName))
                {
                    field = context.SourceEntity.GetFieldSchema(tField.FieldName);
                    tField.FieldId = field.ViewName;
                } else {
                    field = context.SourceEntity.GetFieldSchemaByViewName(tField.FieldId);
                }

                if (field != null && !context.Fields.ContainsKey(tField.FieldId.ToUpper()))
                {
                    var w = BuildWidget(formContext, field);
                    context.Fields.Add(tField.FieldId.ToUpper(), w);
                }
        }

        public BaseWidget BuildWidget(ViewContext.FormContext formContext, BaseField field)
        {   
            var widgetContext = new WidgetContext(formContext);
            widgetContext.Build(field, null);

            var widget = WidgetFactory.Create(widgetContext);
            widget.OnCompile();

            return widget;
        }

        protected override void ExecutePrepareData(DataListContext context, DataListDefinition defn)
        {
            EntityListService service = new EntityListService();
            var data = service.ExecuteData(context.DbQuery, (string fieldName, object val, DynamicObj row) => {
                context.Fields[fieldName].ClearValue();

                context.Fields[fieldName].SetValue(val);
                SetEntityOpenLink(context, defn, row, context.Fields[fieldName]);
                
                return new { FormatedValue = context.Fields[fieldName].FormatedValue, AdditionalValue = context.Fields[fieldName].AdditionalValue == null? null: context.Fields[fieldName].AdditionalValue.CloneData() };
            });

            context.Data = data;
        }
        
        protected void PrepareCell( object value)
        {

        }

        private void SetEntityOpenLink(DataListContext context, DataListDefinition defn, DynamicObj row, BaseWidget widget) 
        {
            if (!String.IsNullOrEmpty(defn.ItemViewField) && widget.WidgetId == defn.ItemViewField)
            {
                widget.SetAdditionalValue(ViewConstant.ViewLink, StackErp.Model.AppLinkProvider.GetDetailPageLink(defn.EntityId, row.Get(ViewConstant.RowId, 0)));                
            }
        }
    }
}
