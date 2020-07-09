using System;
using System.Collections.Generic;
using System.Linq;
using StackErp.Core.Entity;
using StackErp.DB;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.Core
{
    public partial class EntityMetaData
    {
        public static IDictionary<int, DBEntity> entities;

        public static void Build()
        {
            entities = new Dictionary<int, DBEntity>();
            EntityCode.AllEntities = new Dictionary<string, int>();

            try
            {
                var dbentities = DB.EntityDBService.GetEntities();
                var dbentitiesScbhemas = DB.EntityDBService.GetEntitySchemas();
                List<string> avlEntitiesName = new List<string>();
                foreach (var ent in dbentities)
                {
                    var entid = ent.Get("id", 0);
                    var name = ent.Get("name", "");
                    EntityCode.AllEntities.Add(name.ToUpper(), entid);

                    if (avlEntitiesName.Contains(name.ToUpper()))
                        throw new AppException($"Entity with same name {name} found in system");

                    avlEntitiesName.Add(name.ToUpper());

                    var table = ent.Get("tablename", "");
                    Dictionary<string, BaseField> fields = new Dictionary<string, BaseField>();
                    var schemas = dbentitiesScbhemas.Where(x => x.Get("entityid", 0) == entid);
                    foreach (var sch in schemas)
                    {
                        var fname = sch.Get("FIELDNAME", "");

                        if (fields.Keys.Contains(fname.ToUpper()))
                            throw new AppException($"Field with same name <{fname}> in entity <{name}> found in system.");

                        var field = BuildField(name, table, sch, dbentities);
                        fields.Add(fname.ToUpper(), field);
                    }

                    var dbEntity = GetDBEntity(entid, name, fields, table);

                    entities.Add(entid, dbEntity);
                }

                Metadata.FixedEntities.BuildSchema(ref entities);                

                var defItemTypes = DB.EntityDBService.GetDefaultItemTypes();

                foreach (var entK in entities)
                {                    
                    var ent = entK.Value;
                    DynamicObj dataParam = new DynamicObj();

                    var itemTyp = defItemTypes.Where(x => x.Get("entityid", 0) == ent.EntityId.Code);
                    if (itemTyp.Count() > 0) {
                        dataParam.Add("DEFAULTLAYOUT", itemTyp.First().Get("id", 0));
                    }

                    ent.Init(dataParam);
                    foreach (var fieldK in ent.Fields)
                    {
                        fieldK.Value.Init();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new EntityException("Error in building entities. " + ex.Message);
            }

        }

        public static T GetAs<T>(EntityCode id) where T: IDBEntity
        {
            if (entities.Keys.Contains(id.Code))
                return (T)(entities[id.Code] as IDBEntity);

            throw new EntityException($"Requested Entity {id.Code} # {id.Name} not found.");
        }

        public static IDBEntity Get(EntityCode id)
        {
            return GetAs<IDBEntity>(id);
        }

        private static DBEntity GetDBEntity(int entid, string name, Dictionary<string, BaseField> fields, string tableName)
        {
            DBEntity e;
            if (entid == 1) {
                e = new EntityMasterEntity(entid, name, fields,tableName);
            }
            else if (entid == 2) {
                e = new EntitySchemaEntity(entid, name, fields,tableName);
            }
            else if (entid == 101) {
                e = new UserDbEntity(entid, name, fields,tableName);
            } 
            else if (entid == 102) {
                e = new UserRoleEntity(entid, name, fields,tableName);
            } 
            else {
                e = new DBEntity(entid, name, fields, tableName);
            }

            return e;
        }
    }
}
