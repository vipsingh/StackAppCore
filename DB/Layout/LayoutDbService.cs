using System;
using System.Collections.Generic;
using System.Linq;
using StackErp.Model;

namespace StackErp.DB.Layout
{
    public class LayoutDbService
    {
        public static DbObject GetItemType(int entityId, int itemType, int layoutType = 0)
        {
            var entitity = DBService.Single(@"select i.id,i.entityid,i.name,i.code, l.layoutxml from entity_itemtype i
                    join entity_viewlayout l
                    on i.id = l.itemtype
                    where i.entityid=@entityId and i.code='0' and l.viewtype = @layoutType", new {entityId, itemType, layoutType});
            return entitity;
        }
    }
}
