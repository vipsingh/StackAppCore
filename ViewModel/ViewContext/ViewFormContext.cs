using System;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.ViewModel.Model;

namespace StackErp.ViewModel.ViewContext
{
    public class DetailFormContext: FormContext
    {        
        public DetailFormContext(StackAppContext context, EntityCode entity, RequestQueryString requestQuery): base(context, entity, requestQuery)
        {
            this.Entity = (IDBEntity)Core.EntityMetaData.Get(entity);
            IsViewMode = true;
        }

        public override void Build(UIFormModel model = null)
        {
            base.Build();
        }        

        public void CreateDataModel()
        {
            if (IsNew)
                EntityModel = this.Entity.GetDefault();
            else
                EntityModel = this.Entity.GetSingle(this.EntityModelInfo.ObjectId);
        }
    }
}
