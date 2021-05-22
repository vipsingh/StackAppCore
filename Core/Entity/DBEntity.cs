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
using StackErp.Model.Layout;

namespace StackErp.Core
{
    public partial class DBEntity : IDBEntity
    {
        #region Properties
        public int MasterId {get;}
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

        protected string _detailQry;
        private InvariantDictionary<string> _relatedFieldDataQryList;
        #endregion
        public DBEntity(int masterId,
            int id, 
            string name, 
            Dictionary<string, BaseField> fields, 
            EntityType entityType,
            DbObject entityDbo)
        {
            this.MasterId = masterId;
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
        public virtual void Init(DynamicObj dataParam)
        {
            if (_isInit)
                return;

            InitRelations();
            
            DefaultItemTypeId = dataParam.Get("DEFAULTLAYOUT", 0);       

            BuildDefaultQueries();
            PrepareComputeOrderSeq();

            _isInit = true;
        }

        protected virtual void BuildDefaultQueries()
        {
            var qBuilder = new EntityQueryBuilder(this);
            
            _detailQry = qBuilder.BuildDetailQry();

            BuildRelatedFIeldQueries(qBuilder);
        }

        protected void BuildRelatedFIeldQueries(EntityQueryBuilder qBuilder)
        {
            _relatedFieldDataQryList = new InvariantDictionary<string>();
            foreach(var rel in this.Relations)
            {
                if (rel.Type == EntityRelationType.ManyToMany)
                {
                    var childE = EntityMetaData.Get(rel.ChildName);
                    _relatedFieldDataQryList.Add(rel.ParentRefField.Name, qBuilder.PrepareRelatedFieldDataQueries(rel, childE));
                }
            }
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

        public virtual AnyStatus DeleteRecord(StackAppContext appContext, int id)
        {
            //check related data also

            throw new NotImplementedException();
        }

        public virtual Model.Layout.TView GetDefaultLayoutView(EntityLayoutType layoutType)
        {
            return EntityLayoutService.CreateDefault(this, layoutType);
        }

        public virtual EntityListDefinition CreateDefaultListDefn(StackAppContext appContext)
        {         
            var defn = PrepareEntityListDefin();
                           
            var layoutF = this.GetLayoutFields(EntityLayoutType.View);
            var tlist = new TList();
            foreach (var f in layoutF)
            {
                tlist.Fields.Add(new TListField() { FieldId = f.Name });
            }
            defn.Layout = tlist;

            return defn;
        }

        protected EntityListDefinition PrepareEntityListDefin()
        {
            var defn = new EntityListDefinition()
            {
                EntityId = this.EntityId,
                Name = "Default",
                ItemIdField = this.IDField,
                ItemViewField = this.TextField,
                OrderByField  = new List<string>() { this.TextField },
            };

            defn.DataSource = new FieldDataSource() 
            {
                Type = DataSourceType.Entity,
                Entity = this.EntityId
            };

            return defn;
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
