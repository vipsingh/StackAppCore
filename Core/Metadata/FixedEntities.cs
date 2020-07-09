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

            var d = new DBEntity(1, entityName, fs, "t_entitymaster");
            EntityCode.AllEntities.Add(entityName.ToUpper(), 1);

            entities.Add(1, d);

            var entitySchema = BuildEntitySchemaFields();
            EntityCode.AllEntities.Add(entitySchema.Name.ToUpper(), 2);
            entities.Add(2, entitySchema);
        }

        private static DBEntity BuildEntitySchemaFields()
        {
            var entityName = "entityschema";

            var fs = new Dictionary<string, BaseField>();
            
            var list = GetEntitySchemaData();
            foreach(var dbo in list)
            {
                var f = EntityMetaData.BuildField(entityName, entityName, dbo, null);                
                fs.Add(f.Name.ToUpper(), f);
            }
            
            var d = new DBEntity(2, entityName, fs, "t_entityschema");
            d.TextField = "fieldname";
            return d;
        }

        private static List<DbObject> GetEntitySchemaData()
        {
            var list = new List<DbObject>();
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 2,""fieldname"": ""entityid"",""isrequired"": true,""dbname"": ""entityid""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""fieldname"",""isrequired"": true,""dbname"": ""fieldname""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 2,""fieldname"": ""fieldtype"",""isrequired"": true,""dbname"": ""fieldtype"", ""collectionid"": 10}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 2,""fieldname"": ""collectionid"",""isrequired"": false,""dbname"": ""collectionid""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""linkentity"",""isrequired"": false,""dbname"": ""linkentity""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""linkentity_domain"",""isrequired"": false,""dbname"": ""linkentity_domain""}"));            
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""displayexp"",""isrequired"": false,""dbname"": ""displayexp""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""linkentity_field"",""isrequired"": false,""dbname"": ""linkentity_field""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 7,""fieldname"": ""ismultiselect"",""isrequired"": false,""dbname"": ""ismultiselect""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 7,""fieldname"": ""isrequired"",""isrequired"": false,""dbname"": ""isrequired""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""dbname"",""isrequired"": false,""dbname"": ""dbname""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""computeexpression"",""isrequired"": false,""dbname"": ""computeexpression""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""uisetting"",""isrequired"": false,""dbname"": ""uisetting""}"));
            return list;
        }

        private static void AddEntityLayout()
        {
            var entityName = "entityschema";

            var fs = new Dictionary<string, BaseField>();
            
            var list = GetEntitySchemaData();
            foreach(var dbo in list)
            {
                var f = EntityMetaData.BuildField(entityName, entityName, dbo, null);                
                fs.Add(f.Name.ToUpper(), f);
            }
            
            var d = new DBEntity(2, entityName, fs, "t_entityschema");
            d.TextField = "fieldname";
            
        }
    }
}
