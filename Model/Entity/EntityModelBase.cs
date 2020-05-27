using System;
using System.Collections.Generic;
using System.Linq;
using StackErp.Model.Entity;

namespace StackErp.Model
{
    public class DBModelBase
    {
        public string DbTableName {protected set; get;}
        public int MasterId {private set; get; }    
        protected FieldDataCollection _attr;
        public FieldDataCollection Attributes { get => _attr; }
        public Int32 ID { get; protected set; }
        public EntityCode EntityId { get; }
        public int _id { private set; get; }
        public bool IsNew { get => !(this._id > 0); }

        public DBModelBase(EntityCode entityId)
        {
            this.EntityId = entityId;
            _attr = new FieldDataCollection();
        }
        public virtual object GetValue(string field)
        {
            return null;
        }
        
        public virtual void BuiltWithDB(DbObject dbData)
        {
            //this._attr = dbData;
            this.ID = _id = dbData.Get("id", 0);                       
        }

        public void SetMasterId(int masterId)
        {
            this.MasterId = masterId;
        }
    }
    
    public class EntityModelBase: DBModelBase
    {        
        public IDBEntity Entity {get;}
        public short RecordStatus { get; }        
               
        public DateTime CreatedOn {private set; get; }
        public DateTime UpdatedOn {private set; get; }
        public int CreatedBy {private set; get; }
        public int UpdatedBy {private set; get; }            

        public int LayoutId {private set; get; }
        
        public bool HasError { private set; get; }
        public string ErrorMessage { private set; get; }
        public bool IsChangeTrackOff { private set; get; }


        public EntityModelBase(IDBEntity entity): base(entity.EntityId)
        {
            Entity = entity;
            DbTableName = entity.DBName;
        }        

        public virtual void CreateDefault() {
            foreach(var f in Entity.Fields)
            {
                _attr.Add(f.Key, new FieldData(f.Value, f.Value.DefaultValue));
            }
        }

        public override void BuiltWithDB(DbObject dbData)
        {
            base.BuiltWithDB(dbData);
                        
            this.CreatedOn = dbData.Get("createdon", DateTime.MinValue);
            this.UpdatedOn = dbData.Get("updatedon", DateTime.MinValue);
            
            this.SetChangeTrack(true);
            foreach(var f in Entity.Fields)
            {
                var field = f.Value;
                var val = field.ResolveDbValue(dbData);

                _attr.Add(f.Key, new FieldData(f.Value, val){ IsChanged = false });
            }
            this.SetChangeTrack(false);
        }

        public void SetID(int id)
        {
            if(this.ID == 0)
            {
                this.ID = id;
            }
        }
        public bool SetValue(string field, object data) {
            if (_attr.ContainsKey(field.ToUpper())) {
                var f = _attr[field.ToUpper()];
                f.SetValue(data);

                return f.IsValid;
            }

            return false;
        }

        public override object GetValue(string field)
        {
            if (_attr.ContainsKey(field.ToUpper())) {
                var f = _attr[field.ToUpper()];

                return f.Value;
            }

            return null;
        }
        public virtual FieldData GetValueData(string field)
        {
            if (_attr.ContainsKey(field.ToUpper())) {
                var f = _attr[field.ToUpper()];

                return f;
            }

            return null;
        }

        public virtual int CurrentStatusId()
        {
            return 1;
        }

        public virtual void CopyTo(EntityModelBase model)
        {

        }

        public virtual void Validate()
        {

        }

        public virtual List<FieldData> GetInvalidFields()
        {
            return this.Attributes.Where(x => !x.Value.IsValid).Select(x => x.Value).ToList();
        }
        public void SetChangeTrack(bool isOff)
        {
            this.IsChangeTrackOff = isOff;
        }               

    }
}