using System;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Core;

namespace StackErp.Core.Studio
{
    public class StudioService
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

            var d = new DBEntity(9001, "DbEntity", fs);
            EntityCode.AllEntities.Add(entityName.ToUpper(), 9001);

            entities.Add(9001, d);

        }

        private static void BuildEntitySchemaFields()
        {
            var entityName = "entitymaster";

            var fs = new Dictionary<string, BaseField>();
            
            var dbo = DbObject.FromJSON(@"{""fieldtype"": 2,""fieldname"": ""entityid"",""isrequired"": true}");
            var f = EntityMetaData.BuildField(entityName, entityName, dbo, null);            
            fs.Add("entityid", f);

            dbo = DbObject.FromJSON(@"{""fieldtype"": 2,""fieldname"": ""entityid"",""isrequired"": true}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);            
            fs.Add("entityid", f);

            var d = new DBEntity(9002, "EntityField", fs);
            EntityCode.AllEntities.Add(entityName.ToUpper(), 9002);

        }

        public List<DynamicObj> GetAllEntities() 
        {
            var ents = new List<DynamicObj>();

            foreach(var entK in EntityMetaData.entities)
            {
                var ent = entK.Value;
                var o =  new DynamicObj();
                o.Add("Name", ent.Name);
                o.Add("Text", ent.Name);
                o.Add("ID", ent.EntityId.Code);
                ents.Add(o);
            }

            return ents;
        }

        public void GetEntity(int entityId)
        {
            
        }

        public void SaveEntity()
        {

        }
    }
}
