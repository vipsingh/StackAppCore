using System;
using StackErp.Model;
using StackErp.Model.Form;

namespace StackErp.ViewModel.ViewContext
{
    public class ActionContext
    {
        public string Title { set; get; }
        public ActionType Type { set; get; }        
        public RequestQueryString Query { set; get; }
        public string ActionId { set; get; }
        public ActionExecutionType ExecutionType { set; get; }
        private FormContext _FormContext;
        public ActionContext(FormContext formContext, ActionType type, string id)
        {
            _FormContext = formContext;
            Type = type;
            ActionId =id;
        }

        public static ActionContext BuildWithDefinition(FormContext formContext, ActionLinkDefinition definition)
        {
            var cntxt = new ActionContext(formContext, definition.ActionType, definition.ActionId);

            return cntxt;
        }

    }
}
