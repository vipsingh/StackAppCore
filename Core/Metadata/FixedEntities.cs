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
            var dbo = DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""Name"",""dbname"": ""Name"",""isrequired"": true}");
            
            var f = EntityMetaData.BuildField(entityName, entityName, dbo, null);            
            fs.Add("NAME", f);

            dbo = DbObject.FromJSON(@"
                {
                    ""fieldtype"": 1,
                    ""fieldname"": ""text"",
                    ""isrequired"": true,
                    ""dbname"": ""text""
                }
            ");
            
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);            
            fs.Add("TEXT", f);

            dbo = DbObject.FromJSON(@"
                {
                    ""fieldtype"": 1,
                    ""fieldname"": ""tablename"",
                    ""isrequired"": true,
                    ""dbname"": ""tablename""
                }
            ");
            
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);            
            fs.Add("TABLENAME", f);

            dbo = DbObject.FromJSON(@"
                {
                    ""fieldtype"": 1,
                    ""fieldname"": ""primaryfield"",
                    ""isrequired"": true,
                    ""dbname"": ""primaryfield""
                }
            ");
            
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);  
            fs.Add("PRIMARYFIELD", f);

            dbo = DbObject.FromJSON(@"
                {
                    ""fieldtype"": 1,
                    ""fieldname"": ""namefield"",
                    ""isrequired"": true,
                    ""dbname"": ""namefield""
                }
            ");
            
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);            
            fs.Add("NAMEFIELD", f);

            var d = new DBEntity(1, entityName, fs);
            EntityCode.AllEntities.Add(entityName.ToUpper(), 1);

            entities.Add(1, d);

            var entitySchema = BuildEntitySchemaFields();
            EntityCode.AllEntities.Add(entitySchema.Name, 2);
            entities.Add(2, entitySchema);
        }

        private static DBEntity BuildEntitySchemaFields()
        {
            var entityName = "entityschema";

            var fs = new Dictionary<string, BaseField>();
            
            var dbo = DbObject.FromJSON(@"{""fieldtype"": 2,""fieldname"": ""entityid"",""isrequired"": true,""dbname"": ""entityid""}");
            var f = EntityMetaData.BuildField(entityName, entityName, dbo, null);            
            fs.Add("ENTITYID", f);

            dbo = DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""fieldname"",""isrequired"": true,""dbname"": ""fieldname""}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);            
            fs.Add("FIELDNAME", f);

            dbo = DbObject.FromJSON(@"{""fieldtype"": 2,""fieldname"": ""fieldtype"",""isrequired"": true,""dbname"": ""fieldtype""}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);            
            fs.Add("FIELDTYPE", f);

            var d = new DBEntity(2, entityName, fs);
            d.TextField = "fieldname";
            return d;
        }
    }
}
