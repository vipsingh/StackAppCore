using System;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Form;

namespace StackErp.ViewModel.Model
{
    public class PageActions
    {
        public List<ActionInfo> ActionButtons {get;}
        public PageActions() {
            this.ActionButtons = new List<ActionInfo>();
        }
    }
}