using System;
using System.Collections.Generic;
using StackErp.Model.Entity;

namespace StackErp.Model
{
    public class DBModelBase
    {
        protected FieldDataCollection _attr;
        public FieldDataCollection Attributes { get => _attr; }
        public Int32 ID { get; protected set; }
        public EntityCode EntityId { get; }

        public DBModelBase(EntityCode entityId)
        {
            this.EntityId = entityId;
            _attr = new FieldDataCollection();
        }
        public virtual object GetValue(string field)
        {
            return null;
        }
        
        public virtual void BuiltWithDB(DynamicObj dbData)
        {
            //this._attr = dbData;
            this.ID = dbData.Get("id", 0);
        }
    }
    
    public class EntityModelBase: DBModelBase
    {
        public IDBEntity Entity {get;}
        public short RecordStatus { get; }
               
        public DateTime CreatedOn { get; }
        public DateTime UpdatedOn { get; }
        public int CreatedBy { get; }
        public int UpdatedBy { get; }

        public bool IsNew { get => !(this.ID > 0); }

        public int LayoutId { get; }
        
        public bool HasError { private set; get; }
        public string ErrorMessage { private set; get; }


        public EntityModelBase(IDBEntity entity): base(entity.EntityId)
        {
            Entity = entity;
        }

        public void CreateDefault() {
            foreach(var f in Entity.Fields)
            {
                _attr.Add(f.Key, new FieldData(f.Value, f.Value.DefaultValue));
            }
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

        public int CurrentStatusId()
        {
            return 1;
        }

        public void CopyTo(EntityModelBase model)
        {

        }

        public void validate()
        {

        }

        public void GetSaveErrors()
        {

        }

    }
}