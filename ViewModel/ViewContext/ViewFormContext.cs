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
            this.EntityLayoutType = EntityLayoutType.View;
        }

        public override void Build(UIFormModel model = null)
        {
            base.Build();
        }        

        public void CreateDataModel()
        {
            //Add all data related info in this function
            
            if (IsNew)
                EntityModel = this.Entity.GetDefault(this.Context);
            else
                EntityModel = this.Entity.GetSingle(this.Context, this.EntityModelInfo.ObjectId);
        }
    }
}
