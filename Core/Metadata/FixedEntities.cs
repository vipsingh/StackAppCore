using System;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Core;
using StackErp.Core.Entity;

namespace StackErp.Core.Metadata
{
    public class FixedEntities
    {
        public static void BuildSchema(ref IDictionary<int, DBEntity> entities, List<DbObject> dbentities)
        {
            var entityName = "entitymaster";

            var fs = new Dictionary<string, BaseField>();
            var dbo = DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""name"",""dbname"": ""Name"",""isrequired"": true}");
            
            var f = EntityMetaData.BuildField(entityName, entityName, dbo, dbentities);            
            fs.Add("NAME", f);

            dbo = DbObject.FromJSON(@"
                {
                    ""fieldtype"": 1,
                    ""fieldname"": ""text"",
                    ""isrequired"": true,
                    ""dbname"": ""text""
                }
            ");
            
            f = EntityMetaData.BuildField(entityName, entityName, dbo, dbentities);            
            fs.Add("TEXT", f);

            dbo = DbObject.FromJSON(@"
                {
                    ""fieldtype"": 1,
                    ""fieldname"": ""tablename"",
                    ""isrequired"": true,
                    ""dbname"": ""tablename""
                }
            ");
            
            f = EntityMetaData.BuildField(entityName, entityName, dbo, dbentities);            
            fs.Add("TABLENAME", f);

            dbo = DbObject.FromJSON(@"
                {
                    ""fieldtype"": 1,
                    ""fieldname"": ""primaryfield"",
                    ""isrequired"": true,
                    ""dbname"": ""primaryfield""
                }
            ");
            
            f = EntityMetaData.BuildField(entityName, entityName, dbo, dbentities);  
            fs.Add("PRIMARYFIELD", f);

            dbo = DbObject.FromJSON(@"
                {
                    ""fieldtype"": 1,
                    ""fieldname"": ""namefield"",
                    ""isrequired"": true,
                    ""dbname"": ""namefield""
                }
            ");
            
            f = EntityMetaData.BuildField(entityName, entityName, dbo, dbentities);            
            fs.Add("NAMEFIELD", f);


            dbo = DbObject.FromJSON(@"{""fieldtype"": 2,""fieldname"": ""parententity"",""isrequired"": false,""dbname"": ""namefield""}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);
            fs.Add("PARENTENTITY", f); 

            dbo = DbObject.FromJSON(@"{""fieldtype"": 20,""fieldname"": ""fields"",""isrequired"": false,""linkentity"": 2,""linkentity_field"":""entityid""}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);
            fs.Add("FIELDS", f);

            dbo = DbObject.FromJSON(@"{""fieldtype"": 20,""fieldname"": ""layouts"",""isrequired"": false,""linkentity"": 4,""linkentity_field"":""entityid""}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);
            fs.Add("LAYOUTS", f);  

            dbo = DbObject.FromJSON(@"{""fieldtype"": 20,""fieldname"": ""entitylists"",""isrequired"": false,""linkentity"": 5,""linkentity_field"":""entityid""}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);
            fs.Add("ENTITYLISTS", f); 

            dbo = DbObject.FromJSON(@"{""fieldtype"": 20,""fieldname"": ""entityactions"",""isrequired"": false,""linkentity"": 6,""linkentity_field"":""entityid""}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);
            fs.Add("ENTITYACTIONS", f);            

            var entDbo = new DbObject();
            entDbo.Add("tablename", "t_entitymaster");
            var d = new EntityMasterEntity(1, entityName, fs, EntityType.MetadataEntity, entDbo);
            EntityCode.AllEntities.Add(entityName.ToUpper(), 1);

            entities.Add(1, d);

            var entitySchema = BuildEntitySchemaFields(dbentities);
            EntityCode.AllEntities.Add(entitySchema.Name.ToUpper(), 2);
            entities.Add(2, entitySchema);

            AddEntityType(ref entities, dbentities);
            AddEntityList(ref entities);
            AddEntityActions(ref entities);
        }

        private static DBEntity BuildEntitySchemaFields(List<DbObject> dbentities)
        {
            var entityName = "entityschema";

            var fs = new Dictionary<string, BaseField>();
            
            var list = GetEntitySchemaData();
            foreach(var dbo in list)
            {
                var f = EntityMetaData.BuildField(entityName, entityName, dbo, dbentities);                
                fs.Add(f.Name.ToUpper(), f);
            }

            var entDbo = new DbObject();
            entDbo.Add("tablename", "t_entityschema");
            entDbo.Add("namefield", "fieldname");            

            var d = new EntitySchemaEntity(2, entityName, fs, EntityType.MetadataEntity, entDbo);            
            
            return d;
        }

        private static List<DbObject> GetEntitySchemaData()
        {
            var list = new List<DbObject>();
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 2,""fieldname"": ""entityid"",""isrequired"": true,""dbname"": ""entityid""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""fieldname"",""isrequired"": true,""dbname"": ""fieldname""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""label"",""isrequired"": false,""dbname"": ""label""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 9,""fieldname"": ""fieldtype"",""isrequired"": true,""dbname"": ""fieldtype"", ""collectionid"": 3}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 2,""fieldname"": ""length"",""isrequired"": false,""dbname"": ""length""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 2,""fieldname"": ""collectionid"",""isrequired"": false,""dbname"": ""collectionid""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 10,""fieldname"": ""linkentity"",""isrequired"": false,""dbname"": ""linkentity"", ""linkentity"": 1}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""linkentity_domain"",""isrequired"": false,""dbname"": ""linkentity_domain""}"));            
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""displayexp"",""isrequired"": false,""dbname"": ""displayexp""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""linkentity_field"",""isrequired"": false,""dbname"": ""linkentity_field""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 7,""fieldname"": ""ismultiselect"",""isrequired"": false,""dbname"": ""ismultiselect""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 7,""fieldname"": ""isrequired"",""isrequired"": false,""dbname"": ""isrequired""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""dbname"",""isrequired"": false,""dbname"": ""dbname""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""computeexpression"",""isrequired"": false,""dbname"": ""computeexpression""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 24,""fieldname"": ""uisetting"",""isrequired"": false,""dbname"": ""uisetting""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""defaultvalue"",""isrequired"": false,""dbname"": ""defaultvalue""}"));
            list.Add(DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""relatedexp"",""isrequired"": false,""dbname"": ""relatedexp""}"));
            return list;
        }

        private static void AddEntityType(ref IDictionary<int, DBEntity> entities, List<DbObject> dbentities)
        {
            string entityName = "entityitemtype";
            int entId = 3;
            var fs = new Dictionary<string, BaseField>();                       
            
            var entDbo = new DbObject();
            entDbo.Add("tablename", "t_entity_itemtype");
            var d = new DBEntity(0, entId, entityName, fs, EntityType.MetadataEntity, entDbo);            
            
            EntityCode.AllEntities.Add(entityName.ToUpper(), entId);  
            entities.Add(entId, d);

            entityName = "entityviewlayout";
            entId = 4;
            
            var entDbo1 = new DbObject();
            entDbo1.Add("tablename", "t_entity_viewlayout");
            var d1 = new EntityLayoutEntity(entId, entityName, new Dictionary<string, BaseField>(), EntityType.MetadataEntity, entDbo1);            
            
            EntityCode.AllEntities.Add(entityName.ToUpper(), entId);  
            entities.Add(entId, d1);
        }

        private static void AddEntityList(ref IDictionary<int, DBEntity> entities)
        {
            string entityName = "entitylist";
            int entId = 5;
            var fs = new Dictionary<string, BaseField>();                       

            var dbo = DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""name"",""isrequired"": true,""dbname"": ""name""}");
            var f = EntityMetaData.BuildField(entityName, entityName, dbo, null);
            fs.Add("NAME", f);

            dbo = DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""idfield"",""isrequired"": true,""dbname"": ""idfield""}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);
            fs.Add("IDFIELD", f);
            
            dbo = DbObject.FromJSON(@"{""fieldtype"": 2,""fieldname"": ""entityid"",""isrequired"": true,""dbname"": ""idfield"",""viewtype"":-1}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);
            fs.Add("ENTITYID", f);

            dbo = DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""viewfield"",""isrequired"": false,""dbname"": ""viewfield""}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);
            fs.Add("VIEWFIELD", f);
            
            dbo = DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""orderby"",""isrequired"": false,""dbname"": ""orderby""}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);
            fs.Add("ORDERBY", f);

            dbo = DbObject.FromJSON(@"{""fieldtype"": 18,""fieldname"": ""layoutxml"",""isrequired"": true,""dbname"": ""layoutxml""}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);
            fs.Add("LAYOUTXML", f);

            dbo = DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""fixedfilter"",""isrequired"": false,""dbname"": ""fixedfilter""}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);
            fs.Add("FIXEDFILTER", f);

            dbo = DbObject.FromJSON(@"{""fieldtype"": 13,""fieldname"": ""filterpolicy"",""isrequired"": false,""dbname"": ""filterpolicy""}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);
            fs.Add("FILTERPOLICY", f);

            var entDbo = new DbObject();
            entDbo.Add("tablename", "t_entitylist");
            var d = new EntityListEntity(entId, entityName, fs, EntityType.MetadataEntity, entDbo);            
            
            EntityCode.AllEntities.Add(entityName.ToUpper(), entId);  
            entities.Add(entId, d);
        }

        private static void AddEntityActions(ref IDictionary<int, DBEntity> entities)
        {
            string entityName = "entityaction";
            int entId = 6;
            var fs = new Dictionary<string, BaseField>();                       

            var dbo = DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""text"",""isrequired"": true,""dbname"": ""text""}");
            var f = EntityMetaData.BuildField(entityName, entityName, dbo, null);        
            fs.Add("TEXT", f);

            dbo = DbObject.FromJSON(@"{""fieldtype"": 2,""fieldname"": ""entityid"",""isrequired"": true,""dbname"": ""entityid"",""viewtype"":-1}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);
            fs.Add("ENTITYID", f);

            dbo = DbObject.FromJSON(@"{""fieldtype"": 9,""fieldname"": ""viewtype"",""isrequired"": false,""dbname"": ""viewtype"", ""collectionid"": 5}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);
            fs.Add("VIEWTYPE", f);

            dbo = DbObject.FromJSON(@"{""fieldtype"": 9,""fieldname"": ""actiontype"",""isrequired"": false,""dbname"": ""actiontype"", ""collectionid"": 4}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);
            fs.Add("ACTIONTYPE", f);

            dbo = DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""action"",""isrequired"": false,""dbname"": ""action""}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);        
            fs.Add("ACTION", f);

            dbo = DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""queryparam"",""isrequired"": false,""dbname"": ""queryparam""}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);        
            fs.Add("QUERYPARAM", f);            

                        dbo = DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""dataparam"",""isrequired"": false,""dbname"": ""dataparam""}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);        
            fs.Add("DATAPARAM", f);            

                        dbo = DbObject.FromJSON(@"{""fieldtype"": 1,""fieldname"": ""confirmmessage"",""isrequired"": false,""dbname"": ""confirmmessage""}");
            f = EntityMetaData.BuildField(entityName, entityName, dbo, null);        
            fs.Add("CONFIRMMESSAGE", f);            

            var entDbo1 = new DbObject();
            entDbo1.Add("tablename", "t_entityactions");
            entDbo1.Add("namefield", "text");
            var d = new EntityActionEntity(entId, entityName, fs, EntityType.MetadataEntity, entDbo1);                        

            EntityCode.AllEntities.Add(entityName.ToUpper(), entId);  
            entities.Add(entId, d);
        }
    }
}
