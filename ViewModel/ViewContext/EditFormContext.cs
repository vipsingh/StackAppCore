using System;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.ViewModel.Model;

namespace StackErp.ViewModel.ViewContext
{
    public class EditFormContext: FormContext
    {
        public EntityModelBase EntityModel {private set; get;}
        public EditFormContext(StackAppContext context, EntityCode entity, RequestQueryString requestQuery): base(context, entity, requestQuery)
        {
            this.Entity = (IDBEntity)Core.EntityMetaData.Get(entity);
        }

        public override bool IsNew
        {
            get => this.EntityModelInfo.ObjectId <= 0;
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
