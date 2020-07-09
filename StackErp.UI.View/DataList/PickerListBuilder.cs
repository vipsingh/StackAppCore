using System;
using StackErp.Core.DataList;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View.DataList
{
    public class PickerListBuilder: EntityListBuilder
    {
        protected override DataListDefinition GetSourceDefinition(DataListContext context)
        {
            var c = (PickerListContext)context;
            var service = new PickerListService();
             
            return service.GetListDefn(c.DataSource);
        }

        protected override void BuildDataRowActions(DataListContext context, DynamicObj row)
        {

        }
        
    }
}
