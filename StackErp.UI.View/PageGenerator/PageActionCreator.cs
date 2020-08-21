using System;
using System.Linq;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Form;
using StackErp.ViewModel.ViewContext;
using StackErp.ViewModel;

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
            Add(ActionType.Edit, Edit);
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

        private static ActionInfo Edit(ActionContext cntxt)
        {
            var info = CreateDefault(AppLinkProvider.EDIT_ENTITY_URL, cntxt);
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

        public static ActionInfo BuildActionFromDefinition(ActionLinkDefinition definition, FormContext context)
        {
            string url = definition.Action;
            if (definition.ActionType == ActionType.Function)
            {
                url = "";
            }
            
            if(definition.ActionType != ActionType.Page)
            {
                url = GetUrlFromActionType(definition.ActionType);
            }

            url = string.Format("{0}{1}", context.Context.AppRoot, url);

            var q = context.RequestQuery.Clone();
            
            if(definition.QueryParam != null && definition.QueryParam.Count > 0)
            {
                q = new RequestQueryString();
                foreach(var p in definition.QueryParam)
                {
                    q.Add(p.Name, ValueResolver.ResolveValue(context, p.Value).ToString());
                }
            }

            var actionI = new ActionInfo(url, q, definition.ActionId);
            actionI.Title = definition.Text;
            actionI.ActionType = definition.ActionType;
            actionI.ExecutionType = definition.ExecType;

            if (definition.ActionType == ActionType.Function)
            {
                actionI.AddAttribute("Function", definition.Action);
                actionI.ExecutionType = ActionExecutionType.Custom;
            }

            return actionI;                        
        }

        private static string GetUrlFromActionType(ActionType type)
        {
            if (type == ActionType.View)
                return AppLinkProvider.DETAIL_ENTITY_URL;
            else if (type == ActionType.Function)
                return AppLinkProvider.EXEC_FUNC_URL;
            return "";
        }
    }
}
