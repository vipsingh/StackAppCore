using System;
using System.Collections.Generic;

namespace StackErp.Model.Form
{
    public class ActionLinkDefinition
    {
        public ActionType ActionType { set; get; }
        public string ActionId { set; get; }
        public string Visibility { set; get; }
        public EntityCode EntityId { set; get; }
        public EntityLayoutType Viewtype { set; get; }
        public List<int> Operations { set; get; }
        public string Text { set; get; }
        public ActionExecutionType ExecType { set; get; }
        public string Action { set; get; }
        /*map to RequestQuery, if null then by default current RequestQuery*/
        public List<EvalParam> QueryParam { set; get; }
        public List<EvalParam> DataParam { set; get; }
        public string ConfirmMessage { set; get; }
    }
}
