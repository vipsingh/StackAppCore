using System;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Form;

namespace StackErp.ViewModel.Model
{
    public class PageActions
    {
        public InvariantDictionary<ActionInfo> ActionButtons {get;}
        public PageActions() {
            this.ActionButtons = new InvariantDictionary<ActionInfo>();
        }
        public void Add(ActionInfo action)
        {
            if(!ActionButtons.ContainsKey(action.ActionId))
            {
                ActionButtons.Add(action.ActionId, action);
            }
        }
    }
}