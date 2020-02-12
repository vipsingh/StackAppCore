using System;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.ViewModel.Model;

namespace StackErp.ViewModel.ViewContext
{
    public class DeskPageContext: FormContext
    {        
        public DeskPageContext(StackAppContext context, EntityCode entity, RequestQueryString requestQuery): base(context, entity, requestQuery)
        {
            this.Entity = (IDBEntity)Core.EntityMetaData.Get(entity);
            IsViewMode = true;
        }
    }
}
