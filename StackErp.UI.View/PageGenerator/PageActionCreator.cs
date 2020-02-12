using System;
using System.Linq;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Form;
using StackErp.ViewModel.ViewContext;

namespace StackErp.UI.View.PageGenerator
{
    public class PageActionCreator
    {
        static Dictionary<int, Func<ActionContext, ActionInfo>> _actions;
        static PageActionCreator()
        {
            _actions = new Dictionary<int, Func<ActionContext, ActionInfo>>();
            Add(ActionType.Save, Save);
            Add(ActionType.New, New);
        }
        public static void Add(int type, Func<ActionContext, ActionInfo> action)
        {
            if (!_actions.Keys.Contains(type))
                _actions.Add(type, action);
        }
        public static void Add(ActionType type, Func<ActionContext, ActionInfo> action)
        {
            Add((int)type, action);
        }
        public static ActionInfo Create(ActionContext cntxt)
        {
            if (!_actions.Keys.Contains((int)cntxt.Type))
                return null;

            return _actions[(int)cntxt.Type].Invoke(cntxt);
        }
        private static ActionInfo Save(ActionContext cntxt)
        {
            string url = "entity/save";
            var info = CreateDefault(url, cntxt);
            info.ExecutionType = ActionExecutionType.Submit;
            return info;
        }
        private static ActionInfo New(ActionContext cntxt)
        {
            var info = CreateDefault(AppLinkProvider.NEW_ENTITY_URL, cntxt);
            info.ExecutionType = ActionExecutionType.Redirect;
            
            return info;
        }
        static ActionInfo CreateDefault(string url, ActionContext actionContext)
         {
            var actionI = new ActionInfo(url, actionContext.Query, actionContext.ActionId);
            actionI.Title = actionContext.Title;
            actionI.ActionType = actionContext.Type;            
            return actionI;
        }
    }
}
