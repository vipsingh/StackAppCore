using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entity;

namespace DB
{
    public class DBEntity: IDBEntity
    {
        public string Name {set;get;}
        public string DBName {set;get;}
        public Dictionary<string, BaseField> Fields {get; private set;}
        
        private string _fieldText;
        public string FieldText {set { _fieldText = value; } get{ return String.IsNullOrWhiteSpace(_fieldText) ? "Name" : _fieldText; } }

        public List<string> Relations {set;get;}
        public DBEntity(string name, Dictionary<string, BaseField> fields) {
            this.Name = name;
            this.DBName = name.ToLower(); 

            this.Fields = new Dictionary<string, BaseField>();
            this.Fields.Add("ID", new BaseField(){
                Type = FieldType.ObjectKey,
                BaseType = BaseTypeCode.Int32,
                Name = "ID",
                DBName = "ID"                
            });
            this.Fields.Add("CreatedOn", new BaseField(){
                Type = FieldType.Date,
                BaseType = BaseTypeCode.Date,
                Name = "CreatedOn",
                DBName = "CreatedOn" ,
                IsReadOnly= true,
                Copy=false 
            });
            this.Fields.Add("UpdatedOn", new BaseField(){
                Type = FieldType.Date,
                BaseType = BaseTypeCode.Date,
                Name = "UpdatedOn",
                DBName = "UpdatedOn",
                IsReadOnly= true,
                Copy=false 
            });

            foreach(var f in fields){
                this.Fields.Add(f.Key, f.Value);
            }
        }

        public void Init() 
        {

        }

        public EntityRecordModel GetSingle(int id)
        {
            var sql = String.Format("SELECT * FROM {0} WHERE id = @id", this.DBName);
            var arr = DBService.ExecSelectQuery(sql, new {id = id});
            if (arr.Count() > 0) {
                var d = arr.First();
                var model = new EntityRecordModel(this);
                model.BuiltWithDB(d);

                return model;
            }
            throw new EntityException("Record not found.");
        }
    }
}
