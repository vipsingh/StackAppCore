using System;
using System.Collections.Generic;
using StackErp.Core.DataList;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;
using StackErp.Model.Form;
using StackErp.Model.Layout;
using StackErp.UI.View.PageGenerator;
using StackErp.ViewModel;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View.DataList
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
            var formContext = new DetailFormContext(context.Context, context.SourceEntityId, context.Context.RequestQuery);
            formContext.Build();
            //AddField(context, formContext, new TField { FieldId = context.IdColumn }, true);

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

        public BaseWidget BuildWidget(FormContext formContext, BaseField field)
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
            var data = service.ExecuteData(context.DbQuery, 
                (DynamicObj row) => {
                    this.OnPrepareRow(context, defn, row);
                },
                (string fieldName, object val, DynamicObj row) => {
                    return this.OnPrepareCell(context, defn, fieldName, val, row);
            });

            context.Data = data;
        }
        
        protected void OnPrepareRow(DataListContext context, DataListDefinition defn, DynamicObj row)
        {
            BuildDataRowActions(context, row);
        }

        protected object OnPrepareCell(DataListContext context, DataListDefinition defn, string fieldName, object fieldValue, DynamicObj row)
        {
            if (!context.Fields.ContainsKey(fieldName))
                return new { Value = fieldValue };

            context.Fields[fieldName].ClearValue();

            context.Fields[fieldName].SetValue(fieldValue);
            SetEntityOpenLink(context, defn, row, context.Fields[fieldName]);
                
            return new { FormatedValue = context.Fields[fieldName].FormatedValue, AdditionalValue = context.Fields[fieldName].AdditionalValue == null? null: context.Fields[fieldName].AdditionalValue.CloneData() };
        }

        private void SetEntityOpenLink(DataListContext context, DataListDefinition defn, DynamicObj row, IDisplayWidget widget) 
        {
            if (!String.IsNullOrEmpty(defn.ItemViewField) && widget.WidgetId.ToLower() == defn.ItemViewField.ToLower())
            {
                widget.SetAdditionalValue(ViewConstant.ViewLink, StackErp.Model.AppLinkProvider.GetDetailPageLink(defn.DataSource.Entity, row.Get(ViewConstant.RowId, 0)).Url);
            }
        }

        protected virtual void PrepareFilterBar()
        {

        }

        protected override void PrepareLinkDefinition(DataListContext context, DataListDefinition defn)
        {
            //add ActionDefinition
        }

        private void BuildDataRowActions(DataListContext context, DynamicObj row)
        {
            //check operation
            var rowId = row.Get("RowId", 0);
            var actionContext = new ActionContext(null, ActionType.Edit, "BTN_EDIT");
            actionContext.Query = new RequestQueryString();
            actionContext.Query.EntityId = context.SourceEntityId;
            actionContext.Query.ItemId = rowId;
            var ac = PageActionCreator.Create(actionContext);

            var actions = new List<ActionInfo>();
            actions.Add(ac);

            row.Add("RowActions", actions);
        }
    }
}
