using System;
using StackErp.Model.Form;

namespace StackErp.Model
{
    public class AppLinkProvider
    {
        public static ActionInfo GetDetailPageLink(EntityCode entityId, int ItemId)
        {
            var q = new RequestQueryString();
            q.EntityId = entityId;
            q.ItemId = ItemId;
            return new ActionInfo(DETAIL_ENTITY_URL, q, "DETAIL");
        }

        public const string DETAIL_ENTITY_URL = "/entity/detail";
        public const string NEW_ENTITY_URL = "/entity/new";
        public const string EDIT_ENTITY_URL = "/entity/edit";

    }
}
