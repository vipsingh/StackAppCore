using System;
using StackErp.Core.DataList;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.DataList
{
    public class PickerListBuilder: DataListBuilder
    {
        protected override DataListDefinition GetSourceDefinition(DataListContext context)
        {
            var c = (PickerListContext)context;
            var service = new PickerListService();
             
            return service.GetListDefn(c.Field);
        }

        protected override void Compile(DataListContext context, DataListDefinition defn)
        {
            //build header
            base.Compile(context, defn);

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

        }
        protected void PrepareRow(DbObject dbRow)
        {
        }
    }
}
