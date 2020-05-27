using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Form;

namespace StackErp.DB
{
    public static partial class EntityDBService
    {
        public static List<DbObject> GetEntities()
        {
            var entities = DBService.Query("select * from t_entitymaster");
            return entities.ToList();
        }
        public static IEnumerable<DbObject> GetEntitySchemas()
        {
            var entitiesSchemas = DBService.Query("select * from t_entityschema order by id");
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

        public static List<ActionLinkDefinition> GetEntityActions()
        {
            var entitiesActions = DBService.Query("select * from t_entityactions");

            var l = new List<ActionLinkDefinition>();
            foreach(var dbO in entitiesActions)
            {
                var action = new ActionLinkDefinition(){
                    ActionId = "ACT_ENTITY_" + dbO.Get("id", 0).ToString(),
                    ActionType = (ActionType)dbO.Get("actiontype", 0),
                    EntityId = dbO.Get("entityid", 0),
                    Viewtype = (EntityLayoutType)dbO.Get("viewtype", 0),
                    Text = dbO.Get("text", ""),
                    Action = dbO.Get("action", ""),
                };

                action.QueryParam = EvalParam.FromJson(dbO.Get("queryparam", String.Empty));
                action.DataParam = EvalParam.FromJson(dbO.Get("dataparam", String.Empty));

                if (action.EntityId.Code != 0)
                {
                    action.Visibility = FilterExpression.BuildFromJson(action.EntityId, dbO.Get("visibility", String.Empty));
                }

                l.Add(action);
            }
            return l;
        }
    }
}
