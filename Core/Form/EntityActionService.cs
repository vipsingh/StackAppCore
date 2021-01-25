using System;
using System.Collections.Generic;
using System.Linq;
using StackErp.DB;
using StackErp.Model;
using StackErp.Model.Form;

namespace StackErp.Core.Form
{
    public class EntityActionService
    {
        static List<ActionLinkDefinition> _Links;
        static EntityActionService()
        {
            _Links = EntityDBService.GetEntityActions();
            
            // var d = new ActionLinkDefinition()
            // {
            //     ActionId = "ACT_1",
            //     ActionType = ActionType.View,
            //     Viewtype = EntityLayoutType.View,
            //     Text = "View 1",
            //     ExecType = ActionExecutionType.Redirect,
            //     QueryParam = new List<EvalParam>() { 
            //         new EvalParam() { Name= "EntityId", Value= new EvalParamValue(){ Source = EvalSourceType.Constant, Value = 2 } },
            //         new EvalParam() { Name= "ItemId", Value= new EvalParamValue(){ Source = EvalSourceType.ModelField, Value = "Role" } } 
            //         }
            // };
            // _Links.Add(d);

            // d = new ActionLinkDefinition()
            // {
            //     ActionId = "FUN_1",
            //     ActionType = ActionType.Function,
            //     Viewtype = EntityLayoutType.View,
            //     Text = "FUN 1",
            //     ExecType = ActionExecutionType.Custom,
            //     DataParam = new List<EvalParam>() { 
            //         new EvalParam() { Name= "EntityId", Value= new EvalParamValue(){ Source = EvalSourceType.RequestQuery, Value = "EntityId" } },
            //         new EvalParam() { Name= "ItemId", Value= new EvalParamValue(){ Source = EvalSourceType.ModelField, Value = "Role" } } 
            //         }
            // };

            // _Links.Add(d);
        }
        public static ActionLinkDefinition GetViewAction(StackAppContext appContext, EntityCode entity, EntityLayoutType layoutType, int actionId)
        {
            return _Links.Find(x => x.Id == actionId && x.Viewtype == layoutType);
        }

        public static List<ActionLinkDefinition> GetActions(StackAppContext appContext, EntityCode entity, EntityLayoutType layoutType)
        {
            return _Links.Where(x => (x.EntityId.Code == 0 || x.EntityId.Equals(entity)) && x.Viewtype == layoutType).ToList();
        }
    }
}
