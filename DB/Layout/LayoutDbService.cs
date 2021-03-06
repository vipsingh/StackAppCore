using System;
using System.Collections.Generic;
using System.Linq;
using StackErp.Model;

namespace StackErp.DB.Layout
{
    public class LayoutDbService
    {
        private const string GET_ITEMTYPE_LAYOUT = @"select l.masterid, i.id,i.entityid,i.name,i.code, l.layoutxml, l.layoutjson from t_entity_itemtype i
                    join t_entity_viewlayout l
                    on i.id = l.itemtype
                    where i.masterid in(0,@masterId) and i.entityid=@entityId and i.id = @itemType and l.viewtype = @layoutType order by l.masterid desc";
        
        public static DbObject GetItemTypeLayout(int masterId, int entityId, int itemType, int layoutType = 0)
        {
            var entitity = DBService.Single(GET_ITEMTYPE_LAYOUT, new {entityId, itemType, layoutType, masterId});

            return entitity;
        }
    }
}
