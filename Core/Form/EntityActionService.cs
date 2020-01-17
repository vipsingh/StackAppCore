using System;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Form;

namespace StackErp.Core.Form
{
    public class EntityActionService
    {
        private StackAppContext _appContext;
        public EntityActionService(StackAppContext appContext)
        {
            _appContext = appContext;
        }

        public Dictionary<string, ActionLinkDefinition> GetViewActions(string entity, LayoutViewType layoutType)
        {
            return null;
        }
    }
}
