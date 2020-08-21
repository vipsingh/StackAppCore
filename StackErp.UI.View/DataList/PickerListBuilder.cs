using System;
using StackErp.Core;
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

        protected override void Compile(DataListContext context, DataListDefinition defn)
        {
            base.Compile(context, defn);
            this.InsertDependentFields(context, defn);           
        }

        private void InsertDependentFields(DataListContext context, DataListDefinition defn)
        {
            var pickerContext = context as PickerListContext;
            if (pickerContext.FormModelMapping != null)
            {
                foreach(var c in pickerContext.FormModelMapping)
                {                        
                    context.DbQuery.AddField(c.Value.ToString(), true);
                }                
            }
        }

        protected override void BuildDataRowActions(DataListContext context, DynamicObj row)
        {

        }
        
        protected override void OnPrepareRow(DataListContext context, DataListDefinition defn, DynamicObj row, DbObject dataRow)
        {
            var pickerContext = context as PickerListContext;
            if (pickerContext.FormModelMapping != null)
            {
                var mapping = new DynamicObj();
                foreach(var c in pickerContext.FormModelMapping)
                {                        
                    mapping.Add(c.Key, dataRow.Get<object>(c.Value.ToString(), null));
                }                

                row.Add("ModelMapping", mapping);
            }
        }
    }
}
