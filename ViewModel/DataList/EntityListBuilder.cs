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
            
            AddField(context, formContext, new TField { FieldId = context.IdColumn }, true);

            foreach(var lField in defn.Layout.GetLayoutFields())
            {
                AddField(context, formContext, lField);
            }
        }

        protected void AddField(DataListContext context, FormContext formContext, TField tField, bool isHidden = false)
        {
                BaseField field = null;
                field = context.SourceEntity.GetFieldSchema(tField.FieldId);

                if (field != null && !context.Fields.ContainsKey(tField.FieldId))
                {
                    var w = BuildWidget(formContext, field);
                    w.IsHidden = isHidden;
                    context.Fields.Add(tField.FieldId.ToUpper(), w);
                }
                else 
                {
                    context.Fields[tField.FieldId].IsHidden = isHidden;
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
                if (!context.Fields.ContainsKey(fieldName))
                    return new { Value = val };

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
                widget.SetAdditionalValue(ViewConstant.ViewLink, StackErp.Model.AppLinkProvider.GetDetailPageLink(defn.DataSource.Entity, row.Get(ViewConstant.RowId, 0)).Url);
            }
        }

        protected virtual void PrepareFilterBar()
        {

        }
    }
}
