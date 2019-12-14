using System;
using System.Collections.Generic;
using Model.Entity;

namespace Model
{
    public class EntityRecordModel
    {
        public short RecordStatus {get;}

        private DynamicObj _attr;
        public DynamicObj Attributes {get => _attr;}
        public Int32 ID {get;private set;}
        public string EntityName {get;}
        public DateTime CreatedOn {get;}
        public DateTime UpdatedOn {get;}
        public int CreatedBy {get;}
        public int UpdatedBy {get;}

        public bool IsNew {get => !(this.ID > 0);}

        public int LayoutId {get;}        

        public EntityRecordModel(IDBEntity entity) {
            
        }
        public object GetValue(string field) {
            return null;
        }

        public int CurrentStatusId() 
        {
        return 1;
        }

    public void CopyTo(EntityRecordModel model) {

    }

    public void validate() {
        
    }  

        public void BuiltWithDB(DynamicObj dbData) 
        {
            this._attr = dbData;
            this.ID = dbData.Get("id", 0);
        }
    }
}