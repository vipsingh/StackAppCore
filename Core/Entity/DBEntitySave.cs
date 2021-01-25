using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using StackErp.Core.Form;
using StackErp.DB;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.Core
{
    public partial class DBEntity : IDBEntity
    {   
        public virtual AnyStatus Save(StackAppContext appContext, EntityModelBase model)
        {
            return Save(appContext, model, null, null);
        }

        public AnyStatus Save(StackAppContext appContext, EntityModelBase model, IDbConnection connection, IDbTransaction transaction)
        {
            AnyStatus status = AnyStatus.NotInitialized;
            try
            {
                if (model.IsNew)
                {
                    model.SetValue("CREATEDON", DateTime.Now.ToUniversalTime());
                    model.SetValue("CREATEDBY", appContext.UserInfo.UserId);
                    model.SetMasterId(appContext.MasterId);
                }
                model.SetValue("UPDATEDON", DateTime.Now.ToUniversalTime());
                model.SetValue("UPDATEDBY", appContext.UserInfo.UserId);
            
                if (this.Validate(model))
                {
                    if (model is EntityRecordModel)
                    {
                        ((EntityRecordModel)model).ResolveComputedFields();
                    }
                    status = EntityDBService.SaveEntity(appContext, this, model, connection, transaction);
                }
                else 
                {
                    status = AnyStatus.InvalidData;                    
                }
            }
            catch (AppException ex)
            {
                status = AnyStatus.SaveFailure;
                status.Message = ex.Message;
            }
            return status;
        }        

        public virtual AnyStatus OnAfterDbSave(StackAppContext appContext, EntityModelBase model, IDbConnection connection, IDbTransaction transaction)
        {
            AnyStatus sts = AnyStatus.Success;
            
            sts = SaveRelatedData(appContext, model, connection, transaction);
            
            return sts;
        }
        
        private AnyStatus SaveRelatedData(StackAppContext appContext, EntityModelBase model, IDbConnection connection, IDbTransaction transaction)
        {
            AnyStatus sts = AnyStatus.Success;
            //save child entities
            foreach (var f in model.Attributes)
            {               
                var val = f.Value;
                if (val.Field.Type == FieldType.OneToMany && val.IsChanged && val.Value != null && (val.Value as IList).Count > 0)
                {
                    var itemColl = (List<EntityModelBase>)val.Value;
                    var field = val.Field;
                    var entity = GetEntity(field.RefObject);
                    foreach (var localModel in itemColl)
                    {
                        SetRelationshipValue(model.ID, field.Name, localModel);
                        sts = entity.Save(appContext, localModel, connection, transaction);
                        if (sts != AnyStatus.Success)
                        {
                            break;
                        }
                    }
                }
            }

            return sts;
        }
        private void SetRelationshipValue(int parentId, string parentFieldName, EntityModelBase childModel)
        {
            var rel = this.Relations.Find(x => x.ParentRefField.Name == parentFieldName);
            childModel.SetValue(rel.ChildRefField.Name, parentId);
        }

        public virtual AnyStatus OnBeforeDbSave(StackAppContext appContext, EntityModelBase model, IDbConnection connection, IDbTransaction transaction)
        {
            var sts = AnyStatus.Success;
            if (model is EntityRecordModel)
                ((EntityRecordModel)model).PrepareSaveImageField(appContext);
            return sts;
        }

        protected virtual bool Validate(EntityModelBase model)
        {
            //check concurrency based on LastModifiedOn
            bool IsValid = true;            
            var validator = new DataValidator();
            foreach (var f in model.Attributes)
            {
                var field = f.Value;
                if(model.IsNew || field.IsChanged)
                {
                    if(field.Field.IsRequired && !validator.HasValue(field.Field, field.Value))
                    {
                        field.ErrorMessage = $"Field {field.Field.Text} is required";
                        field.IsValid = false;
                    }
                }

                if(!field.IsValid)
                    IsValid = false;
            }

            return IsValid;
        }
    }
}