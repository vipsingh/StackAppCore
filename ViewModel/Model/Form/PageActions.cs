using System;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Form;

namespace StackErp.ViewModel.Model
{
    public class PageActions
    {
        public Dictionary<string, ActionInfo> ActionButtons {get;}
        public PageActions() {
            this.ActionButtons = new Dictionary<string, ActionInfo>();
        }
        public void Add(ActionInfo action)
        {
            if(!ActionButtons.ContainsKey(action.ActionId))
            {
                ActionButtons.Add(action.ActionId.ToUpper(), action);
            }
        }
    }
}