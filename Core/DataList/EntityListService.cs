using System;
using System.Collections;
using System.Collections.Generic;
using StackErp.Core.Layout;
using StackErp.DB.DataList;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;
using StackErp.Model.Layout;

namespace StackErp.Core.DataList
{
    public class EntityListService
    {
        public static DataListDefinition GetEntityListDefn(StackAppContext appContext, EntityCode entityId, int queryId = 0)
        {
            var deff = ListDbService.GetEntityList(entityId);
            var _Entity = Core.EntityMetaData.Get(entityId);
            if (deff == null) 
            {
                deff = _Entity.CreateDefaultListDefn(appContext);
            }
            deff.Id = entityId.Code.ToString() + "_" + queryId.ToString();
            if (deff.PageSize <= 0)
                deff.PageSize = 50;

            return deff;
        }        

        public static IEnumerable<DbObject> ExecuteData(StackAppContext appContext, DbQuery query)
        {
            var data = QueryDbService.ExecuteEntityQuery(appContext, query);
            return data;
        }
    }
}
