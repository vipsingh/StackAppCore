using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.DB
{
    public static partial class EntityDBService
    {

        public static List<DbObject> GetEntities()
        {
            var entities = DBService.Query("select * from entitymaster");
            return entities.ToList();
        }
        public static IEnumerable<DbObject> GetEntitySchemas()
        {
            var entitiesSchemas = DBService.Query("select * from entityschema");
            return entitiesSchemas;
        }

        public static int GetNextEntityDBId(int entityId)
        {
            var currId = DBService.Single("select max(MaxID) as a from AUTOID where EntityId=@EntityId", new { EntityId = entityId });
            var cid = currId.Get("a", -1);
            if (cid == -1)
            {
                DBService.Execute("INSERT INTO AUTOID VALUES(@EntId,@Id)", new { EntId = entityId, Id = 1 });
                return 1;
            }
            else
            {
                DBService.Execute("UPDATE AUTOID SET MaxID=@Id WHERE EntityId=@EntId", new { EntId = entityId, Id = cid + 1 });
                return cid + 1;
            }
        }
    }
}
