using System;
using System.Collections.Generic;
using System.Linq;
using StackErp.DB;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.Core
{
    public class EntityMetaData
    {
        private static IDictionary<string, DBEntity> entities;

        public static IDictionary<string, DBEntity> B() 
        {
            entities = new Dictionary<string, DBEntity>();
            
            var dbentities = DB.EntityDBService.GetEntities();
            var dbentitiesScbhemas = DB.EntityDBService.GetEntitySchemas();
            foreach(var ent in dbentities) 
            {                
                var entid = ent.Get("id", 0); 
                var name = ent.Get("name", "");
                var table = ent.Get("tablename", "");
                Dictionary<string,BaseField> fields = new Dictionary<string, BaseField>();
                var schemas = dbentitiesScbhemas.Where(x =>  x.Get("entityid",0) == entid);
                foreach(var sch in schemas) 
                {
                    var fname = sch.Get("FIELDNAME", "");
                    var field = BuildField(name, table, sch, dbentities);
                    fields.Add(fname.ToUpper(), field);
                }
                
                    var e = new DBEntity(name, fields);                    
                    entities.Add(name.ToUpper(), e);            
            }

            foreach(var ent in entities) {
                ent.Value.Init();
            }

            return entities;
        }

        static BaseTypeCode GetBaseType(FieldType typ) {
            BaseTypeCode t = BaseTypeCode.String;
            switch(typ) {
                case FieldType.BigInt:
                    t= BaseTypeCode.Int64;
                    break;
                case FieldType.Bool:
                    t= BaseTypeCode.Boolean;
                    break;
                case FieldType.Date:
                    t= BaseTypeCode.DateTime;
                    break;
                case FieldType.DateTime:
                    t= BaseTypeCode.DateTime;
                    break;
                case FieldType.Decimal:
                    t= BaseTypeCode.Decimal;
                    break;
                case FieldType.Email:
                    t= BaseTypeCode.String;
                    break;
                case FieldType.Integer:
                    t= BaseTypeCode.Int32;
                    break;
                    
                case FieldType.LongText:                
                    t= BaseTypeCode.String;
                    break;                    
                
            }

            return t;
        }        

        private static BaseField BuildField(string entName, string table, DynamicObj sch, List<DynamicObj> dbentities) 
        {
            var field = new BaseField();
                    var typ = sch.Get("fieldtype", 0);
                    var fname = sch.Get("FIELDNAME", "");
                    field.Name = fname;
                    field.Type = (FieldType)typ;
                    field.BaseType = GetBaseType(field.Type);
                    //field.Entity = name;
                    field.DBName = fname;
                    field.IsDbStore = true;
                    field.TableName =table;
                    
                    if(field.Type == FieldType.ObjectLink) 
                    {
                        var linkEnt = sch.Get("linkentity", 0);
                        var linkDbEnt = dbentities.Where(x => x.Get("id", 0) == linkEnt);
                        if (linkDbEnt.Count() > 0) 
                        {
                            field.RefObject = linkDbEnt.First().Get("name", "");
                        }

                        field.DBName = fname + "__id";
                    }

                return field;
        }
        public static DBEntity Get(string name) {
             return entities[name.ToUpper()];
        }

    }
}
