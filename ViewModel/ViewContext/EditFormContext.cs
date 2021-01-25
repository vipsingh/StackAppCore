using System;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ValueProvider;

namespace StackErp.ViewModel.ViewContext
{
    public class EditFormContext: FormContext
    {        
        public EditFormContext(StackAppContext context, EntityCode entity, RequestQueryString requestQuery): base(context, entity, requestQuery)
        {
            this.Entity = (IDBEntity)Core.EntityMetaData.Get(entity);
            this.EntityLayoutType = EntityLayoutType.Edit;
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
            //Add all data related info in this function
            
            if (IsNew)
            {
                EntityModel = this.Entity.GetDefault(this.Context);
                EntityModel.SetValue(ViewConstant.ItemType, this.EntityModelInfo.ItemType);
                SetRelationShip();
            }
            else
                EntityModel = this.Entity.GetSingle(this.Context, this.EntityModelInfo.ObjectId);
        }

        public void PrepareLinkedData(CustomRequestInfo requestInfo)
        {
            if (requestInfo.DependencyContext != null && requestInfo.DependencyContext.RefData != null)
            {
                var valueProvider = new FormValueProvider(this);
                var refrences = new DynamicObj();
                foreach(var d in requestInfo.DependencyContext.RefData)
                {
                    object val;
                    valueProvider.ResolveFieldValue(d.Value, out val);
                    refrences.Add(d.Key, val);
                }

                this.AddParameter(ViewConstant.LinkedData, refrences);
            }
        }

        private void SetRelationShip()
        {
            if (this.RequestQuery.RelatedEntityId.Code > 0 && this.RequestQuery.RelatedObjectId > 0)
            {
                EntityModel.SetRelationValue(this.RequestQuery.RelationField, this.RequestQuery.RelatedEntityId, this.RequestQuery.RelatedObjectId);
            }
        }
    }
}
