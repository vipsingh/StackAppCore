using System;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Form;
using StackErp.Model.Utils;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.Model
{
    public class ViewPage
    {
        public Dictionary<string, BaseWidget> Controls {get;set;}        
        public string PostUrl {get;}
        public Dictionary<string, ActionInfo> Actions {get;set;}
        public ObjectModelInfo EntityInfo {set;get;}
        public string ErrorMessage {set;get;}

        public object Layout {set;get;}
        public ViewPage(FormContext formContext)
        {
            this.EntityInfo = formContext.EntityModelInfo;
            this.Controls = formContext.Widgets; 
            this.Actions = formContext.Actions.ActionButtons;

            this.PostUrl = formContext.Context.AppRoot + "entity/save?" + formContext.RequestQuery.ToQueryString();
        }
    }
}