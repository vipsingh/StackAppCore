using System;
using StackErp.Core.DataList;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;
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

            foreach(var tField in defn.Layout.Fields)
            {
                BaseField field = null;
                if (!String.IsNullOrEmpty(tField.FieldName))
                {
                    field = context.SourceEntity.GetFieldSchema(tField.FieldName);
                } else {
                    field = context.SourceEntity.GetFieldSchemaByViewName(tField.FieldId);
                }

                if (field != null)
                {
                    var w = BuildWidget(formContext, field);
                    context.Fields.Add(tField.FieldId.ToUpper(), w);
                }
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
            var data = service.ExecuteData(context.DbQuery);
            context.Data = data;
        }
        protected void PrepareRow(DbObject dbRow)
        {

        }
    }
}
