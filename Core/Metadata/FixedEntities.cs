using System;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Core;

namespace StackErp.Core.Metadata
{
    public class FixedEntities
    {
        public static void BuildSchema(ref IDictionary<int, DBEntity> entities)
        {
            var entityName = "entitymaster";

            var fs = new Dictionary<string, BaseField>();
            var dbo = DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""Name"",""isrequired"": true}");
            
            var f = EntityMetaData.BuildField(entityName, entityName, dbo, null);            
            fs.Add("NAME", f);

            dbo = DbObject.FromJSON(@"
                {
                    ""fieldtype"": 1,
                    ""fieldname"": ""text"",
                    ""isrequired"": true
                }
            ");
            
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);            
            fs.Add("TEXT", f);

            dbo = DbObject.FromJSON(@"
                {
                    ""fieldtype"": 1,
                    ""fieldname"": ""tablename"",
                    ""isrequired"": true
                }
            ");
            
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);            
            fs.Add("TABLENAME", f);

            dbo = DbObject.FromJSON(@"
                {
                    ""fieldtype"": 1,
                    ""fieldname"": ""primaryfield"",
                    ""isrequired"": true
                }
            ");
            
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);  
            fs.Add("PRIMARYFIELD", f);

            dbo = DbObject.FromJSON(@"
                {
                    ""fieldtype"": 1,
                    ""fieldname"": ""namefield"",
                    ""isrequired"": true
                }
            ");
            
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);            
            fs.Add("NAMEFIELD", f);

            var d = new DBEntity(9001, entityName, fs);
            EntityCode.AllEntities.Add(entityName.ToUpper(), 9001);

            entities.Add(9001, d);

            var entitySchema = BuildEntitySchemaFields();
            EntityCode.AllEntities.Add(entitySchema.Name, 9002);
            entities.Add(9002, entitySchema);
        }

        private static DBEntity BuildEntitySchemaFields()
        {
            var entityName = "entityschema";

            var fs = new Dictionary<string, BaseField>();
            
            var dbo = DbObject.FromJSON(@"{""fieldtype"": 2,""fieldname"": ""entityid"",""isrequired"": true}");
            var f = EntityMetaData.BuildField(entityName, entityName, dbo, null);            
            fs.Add("ENTITYID", f);

            dbo = DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""fieldname"",""isrequired"": true}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);            
            fs.Add("FIELDNAME", f);

            dbo = DbObject.FromJSON(@"{""fieldtype"": 2,""fieldname"": ""fieldtype"",""isrequired"": true}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);            
            fs.Add("FIELDTYPE", f);

            var d = new DBEntity(9002, entityName, fs);            
            d.TextField = "fieldname";
            return d;
        }
    }
}
