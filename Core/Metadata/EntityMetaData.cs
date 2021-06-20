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
                    var name = ent.Get("name", "");
                    if (avlEntitiesName.Contains(name.ToUpper()))
                        throw new AppException($"Entity with same name {name} found in system");

                    avlEntitiesName.Add(name.ToUpper());

                    DBEntity dbEntity = BuildEntity(dbentities, dbentitiesScbhemas, ent);
                    int entid = dbEntity.EntityId.Code;

                    entities.Add(entid, dbEntity);
                }

                Metadata.FixedEntities.BuildSchema(ref entities, dbentities);                

                var defItemTypes = DB.EntityDBService.GetDefaultItemTypes();

                foreach (var entK in entities)
                {
                    var itemTyp = defItemTypes.Where(x => x.Get("entityid", 0) == entK.Value.EntityId.Code);
                    var defItmType = 0;
                    if (itemTyp.Count() > 0)
                    {
                        defItmType = itemTyp.First().Get("id", 0);
                    }

                    InitEntity(defItmType, entK.Value);
                }
            }
            catch (Exception ex)
            {
                throw new EntityException("Error in building entities. " + ex.Message);
            }

        }

        private static void InitEntity(int itemTyp, DBEntity ent)
        {
            DynamicObj dataParam = new DynamicObj();

            dataParam.Add("DEFAULTLAYOUT", itemTyp);

            foreach (var fieldK in ent.Fields)
            {
                fieldK.Value.ControlType = GetDefaultControl(fieldK.Value.Type);
                fieldK.Value.Init();
            }

            ent.Init(dataParam);
        }

        private static DBEntity BuildEntity(List<DbObject> dbentities, IEnumerable<DbObject> dbentitiesScbhemas, DbObject ent)
        {
            var entMasterId = ent.Get("masterid", 0);
            var entid = ent.Get("id", 0);
            var name = ent.Get("name", "");
            EntityCode.AllEntities.Remove(name.ToUpper());
            EntityCode.AllEntities.Add(name.ToUpper(), entid);

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

            var entityType = EntityType.CoreEntity;
            var parententity = ent.Get("parententity", 0);
            if (parententity > 0)
            {
                entityType = EntityType.ChildEntity;
            }

            var hasstages = ent.Get("hasstages", false);
            if (hasstages)
            {
                fields.Add("STAGEID", new IntegerField()
                {
                    Type = FieldType.Integer,
                    Name = "stageid",
                    DBName = "stageid",
                    IsReadOnly = true,
                    IsDbStore = true,
                    ViewId = -1
                });

                fields.Add("STATUSID", new IntegerField()
                {
                    Type = FieldType.Integer,
                    Name = "statusid",
                    DBName = "statusid",
                    Text = "Status",
                    IsReadOnly = true,
                    IsDbStore = true,
                    ViewId = 0
                });
            }

            var hasimage = ent.Get("hasimage", false);
            if (hasimage)
            {
                fields.Add("OBJECTIMAGE", new ImageField()
                {
                    Type = FieldType.Integer,
                    Name = "objectimage",
                    DBName = "objectimage",
                    Text = "Image",
                    IsReadOnly = true,
                    IsDbStore = true,
                    ViewId = 0
                });
            }

            var dbEntity = GetDBEntity(10, entid, name, fields, entityType, ent);
            dbEntity.ParentEntity = parententity;
            dbEntity.HasStages = hasstages;
            if (hasstages)
                dbEntity.StageGroupId = ent.Get("stagegroupid", 0);

            dbEntity.EntityFeatures = ent.Get("features", "");

            return dbEntity;
        }

        //public static void BuildEntity(int entityid, int defaultItemType)
        //{
        //    var dbentities = DB.EntityDBService.GetEntities(entityid);
        //    var dbentitiesScbhemas = DB.EntityDBService.GetEntitySchemas(entityid);
        //    DBEntity dbEntity = null;
        //    foreach (var ent in dbentities)
        //    {
        //        var name = ent.Get("name", "");
        //        dbEntity = BuildEntity(dbentities, dbentitiesScbhemas, ent);
        //        int entid = dbEntity.EntityId.Code;

        //        entities.Remove(entid);
        //        entities.Add(entid, dbEntity);
        //    }

        //    InitEntity(defaultItemType, dbEntity);
        //}
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

        private static DBEntity GetDBEntity(int masterId, int entid, string name, Dictionary<string, BaseField> fields, EntityType entityType, DbObject entityDbo)
        {
            DBEntity e;
            if (entid == 101) {
                e = new UserDbEntity(masterId, entid, name, fields,entityType, entityDbo);
            } 
            else if (entid == 102) {
                e = new UserRoleEntity(masterId, entid, name, fields,entityType, entityDbo);
            } 
            else {
                e = new DBEntity(masterId, entid, name, fields, entityType, entityDbo);
            }

            return e;
        }
    }
}
