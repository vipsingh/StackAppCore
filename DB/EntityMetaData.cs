using System;
using System.Collections.Generic;
using Model;
using Model.Entity;

namespace DB
{
    public class EntityMetaData
    {
        private static IDictionary<string, DBEntity> entities;
        public static void Build() {
            entities = new Dictionary<string, DBEntity>();

            var fs = new Dictionary<string, BaseField>();
            fs.Add("LoginId", new BaseField(){
                Type = FieldType.Text,
                BaseType = BaseTypeCode.String,
                Name = "LoginId",
                DBName = "LoginId"                
            });
            fs.Add("SubmitAmount", new BaseField(){
                Type = FieldType.Decimal,
                BaseType = BaseTypeCode.Decimal,
                Name = "SubmitAmount",
                DBName = "SubmitAmount",
                DecimalPlace=2                
            });
            fs.Add("AssignDate", new BaseField(){
                Type = FieldType.Date,
                BaseType = BaseTypeCode.Date,
                Name = "AssignDate",
                DBName = "AssignDate"  
            });

            var e = new DBEntity("UserMaster", fs);           

            entities.Add("usermaster", e);            

        }

        public static DBEntity Get(string name) {
             return entities[name.ToLower()];
        }

    }
}
