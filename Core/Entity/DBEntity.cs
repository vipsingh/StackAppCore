using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using StackErp.Core.Form;
using StackErp.Core.Layout;
using StackErp.DB;
using StackErp.DB.DataList;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;

namespace StackErp.Core
{
    public partial class DBEntity : IDBEntity
    {
        #region Properties
        public EntityCode EntityId { get; }
        public AppModuleCode AppModule { get; }
        public string Name { get; }
        public string DBName { private set; get; }
        public string Text { private set; get; }
        public InvariantDictionary<BaseField> Fields { get; private set; }

        private string _fieldText;
        public string TextField { protected set { _fieldText = value; } get { return String.IsNullOrWhiteSpace(_fieldText) ? "Name" : _fieldText; } }

        public string IDField { get; private set; }
        public List<IEntityRelation> Relations { private set; get; }

        public List<string> ComputeOrderSeq { private set; get; }

        //If true than it can not be used directly
        public EntityType EntityType {protected set;get;}
        public EntityDeletePolicyType DeletePolicyType {protected set;get;}

        public int DefaultItemTypeId {private set;get;} 
        public int ParentEntity {set;get;} 
        public bool HasStages {set;get;} 
        public int StageGroupId {set;get;}
        public string EntityFeatures {set;get;}

        private string _detailQry;
        private InvariantDictionary<string> _relatedFieldDataQryList;
        #endregion
        public DBEntity(int id, 
            string name, 
            Dictionary<string, BaseField> fields, 
            EntityType entityType,
            DbObject entityDbo)
        {
            this.EntityId = id;
            this.Name = name;
            this.DBName = entityDbo.Get("tablename", ""); //t_{namespace}
            this.IDField = "ID";
            this.Text = name;
            this.EntityType = entityType;

            _fieldText = entityDbo.Get("namefield", "");

            this.Fields = new InvariantDictionary<BaseField>();

            #region Default Fields
            if (fields.Where(f => f.Key.ToUpper() == "ID").Count() == 0)
            {
                this.Fields.Add("ID", new ObjectKeyField()
                {
                    Type = FieldType.ObjectKey,
                    Name = "ID",
                    DBName = "ID",
                    IsReadOnly = true,
                    Entity = this,
                    IsDbStore = true
                });
            }

            fields.Add("CREATEDON", new DateTimeField()
            {
                Type = FieldType.DateTime,
                Name = "CreatedOn",
                DBName = "CreatedOn",
                IsReadOnly = true,
                Copy = false,
                IsDbStore = true,
                ViewId = -1
            });
            fields.Add("UPDATEDON", new DateTimeField()
            {
                Type = FieldType.DateTime,
                BaseType = TypeCode.DateTime,
                Name = "UpdatedOn",
                DBName = "UpdatedOn",
                IsReadOnly = true,
                Copy = false,
                IsDbStore = true,
                ViewId = -1
            });
            fields.Add("CREATEDBY", new IntegerField()
            {
                Type = FieldType.Integer,
                Name = "CreatedBy",
                DBName = "CreatedBy",
                IsReadOnly = true,
                Copy = false,
                IsDbStore = true,
                ViewId = -1
            });
            fields.Add("UPDATEDBY", new IntegerField()
            {
                Type = FieldType.Integer,
                Name = "UpdatedBy",
                DBName = "UpdatedBy",
                IsReadOnly = true,
                Copy = false,
                IsDbStore = true,
                ViewId = -1
            });

            if (entityType == EntityType.CoreEntity)
            {
                fields.Add("ITEMTYPE", new IntegerField()
                {
                    Type = FieldType.Integer,
                    Name = "ItemType",
                    DBName = "itemtype",
                    IsReadOnly = true,
                    IsDbStore = true,
                    ViewId = -1
                });
            }
            #endregion

            var orderedFields = fields.Select(x => x.Value).OrderBy(x => x.ViewOrder);

            foreach (var f in orderedFields)
            {
                if (!this.Fields.Keys.Contains(f.Name.ToUpper()))
                {
                    f.Entity = this;
                    this.Fields.Add(f.Name.ToUpper(), f);
                }
            }

            this.Relations = new List<IEntityRelation>();
            //this.DbConstraint
        }

        private bool _isInit = false;
        public void Init(DynamicObj dataParam)
        {
            if (_isInit)
                return;

            InitRelations();
            
            DefaultItemTypeId = dataParam.Get("DEFAULTLAYOUT", 0);

            var qBuilder = new EntityQueryBuilder(this);
            
            _detailQry = qBuilder.BuildDetailQry();
            _relatedFieldDataQryList = new InvariantDictionary<string>();
            foreach(var rel in this.Relations)
            {
                if (rel.Type == EntityRelationType.ManyToMany)
                {
                    var childE = EntityMetaData.Get(rel.ChildName);
                    _relatedFieldDataQryList.Add(rel.ParentRefField.Name, qBuilder.PrepareRelatedFieldDataQueries(rel, childE));
                }
            }

            PrepareComputeOrderSeq();

            _isInit = true;
        }

        private void InitRelations()
        {
            foreach (var field in this.Fields)
            {
                var fieldInfo = field.Value;

                if (fieldInfo.Type == FieldType.ObjectLink)
                {
                    var childE = EntityMetaData.Get(fieldInfo.RefObject);
                    var rel = new EntityRelation(EntityRelationType.ManyToOne, this.EntityId, fieldInfo.RefObject, fieldInfo, null, childE.GetFieldSchema(childE.TextField)); 
                    this.Relations.Add(rel);
                }
                if (fieldInfo.Type == FieldType.MultiObjectLink)
                {
                    var childE = EntityMetaData.Get(fieldInfo.RefObject);
                    var rel = new EntityRelation(EntityRelationType.ManyToMany, this.EntityId, fieldInfo.RefObject, fieldInfo, null, childE.GetFieldSchema(childE.TextField));
                    this.Relations.Add(rel);
                }
                else if (fieldInfo.Type == FieldType.OneToMany)
                {
                    var relField = (OneToManyField)fieldInfo;
                    var childE = EntityMetaData.Get(relField.RefObject);
                    var rel = new EntityRelation(EntityRelationType.OneToMany, this.EntityId, relField.RefObject, fieldInfo, childE.GetFieldSchema(relField.RefFieldName), null);
                    this.Relations.Add(rel);
                }
                // else if (fieldInfo.Type == FieldType.OneToOne)
                // {
                //     var relField = (OneToManyField)fieldInfo;
                //     var childE = EntityMetaData.Get(relField.RefObject);
                //     var rel = new EntityRelation(EntityRelationType.OneToMany, this.EntityId, relField.RefObject, fieldInfo, childE.GetFieldSchema(relField.RefFieldName), null);
                //     this.Relations.Add(rel);
                // }
            }
        }

        private void PrepareComputeOrderSeq()
        {
            var comFields = this.Fields.Where(f => f.Value.IsComputed).Select(x => x.Value);
            if (comFields.Count() == 0)
                this.ComputeOrderSeq = new List<string>();
            var seqArr = comFields.Select(f => f.Name.ToLower()).ToList();            

            foreach(var f in comFields)
            {
                    var keys = f.ComputeExpression.KeyWords;
                    foreach(var k in keys)
                    {
                        if (seqArr.IndexOf(k) > seqArr.IndexOf(f.Name.ToLower()))
                            DataHelper.Swap(seqArr, seqArr.IndexOf(k), seqArr.IndexOf(f.Name.ToLower()));
                    }                
            }

            this.ComputeOrderSeq = seqArr;
        }
        
        #region Schema related
        public Dictionary<string, BaseField> GetFields()
        {
            return this.Fields;
        }

        public List<BaseField> GetLayoutFields(EntityLayoutType type)
        {
            var fs = new List<BaseField>();
            foreach (var f in this.Fields)
            {                                
                if (f.Value.ViewId == 0 || f.Value.ViewId == Convert.ToInt16(type))
                {
                    fs.Add(f.Value);
                }
            }

            return fs;
        }

        public BaseField GetFieldSchema(string fieldName)
        {
            if (this.Fields.ContainsKey(fieldName.ToUpper()))
            {
                return this.Fields[fieldName.ToUpper()];
            }

            return null;
        }
        public BaseField GetFieldSchema(int fieldId)
        {
            var f = this.Fields.Where(x => x.Value.FieldId == fieldId);
            return f.Count() > 0? f.First().Value : null;
        }
        // public BaseField GetFieldSchemaByViewName(string fieldViewName)
        // {
        //     var res = this.Fields.Where(x => x.Value.ViewName == fieldViewName);
        //     if (res.Count() > 0)
        //         return res.First().Value;

        //     return null;
        // }
        #endregion

        public IDBEntity GetEntity(EntityCode id)
        {
            return Core.EntityMetaData.Get(id);
        }

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

        public virtual AnyStatus DeleteRecord(int id)
        {
            //check related data also

            throw new NotImplementedException();
        }

        public virtual Model.Layout.TView GetDefaultLayoutView(EntityLayoutType layoutType)
        {
            return EntityLayoutService.CreateDefault(this, layoutType);
        }

        //private Hooks
        /*
        EntityModelHooksType
        */

    }

    public class EntityRelation : IEntityRelation
    {
        public EntityRelationType Type { private set; get; }
        public EntityCode ParentName { private set; get; }
        public EntityCode ChildName { private set; get; }
        public BaseField ParentRefField { private set; get; }
        public BaseField ChildRefField { private set; get; }
        //Used in Link Field to display data
        public BaseField ChildDisplayField { private set; get; }

        public List<(string, string)> OtherChildFields { set; get; }

        public EntityRelation(EntityRelationType type, EntityCode parent, EntityCode child, BaseField parentField, BaseField childField, BaseField childDisplayField)
        {
            Type = type;
            ParentName = parent;
            ChildName = child;
            ParentRefField = parentField;
            ChildRefField = childField;
            ChildDisplayField = childDisplayField;
        }
    }
}
