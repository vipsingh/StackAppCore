using System;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Form;
using StackErp.Model.Layout;
using StackErp.Model.Utils;
using StackErp.ViewModel.FormWidget;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.Model
{
    public class ViewPage
    {
        public AppPageType PageType {set;get;}
        public Dictionary<string, IWidget> Widgets {get;set;}   
        public List<FormRule> FormRules {get;set;}   
        public string PostUrl {get;}
        public InvariantDictionary<ActionInfo> Actions {get;set;}
        public ObjectModelInfo EntityInfo {set;get;}
        public DynamicObj PageTitle {set;get;}
        public string ErrorMessage {set;get;}

        public TView Layout {set;get;}
        public ViewPage(FormContext formContext)
        {
            PageType = AppPageType.Detail;
                        
            this.Widgets = formContext.Widgets; 

            this.PostUrl = formContext.Context.AppRoot + "entity/save?" + formContext.RequestQuery.ToQueryString();
        }

        public ViewPage()
        {
            PageType = AppPageType.Detail;
            this.Widgets = new Dictionary<string, IWidget>();
        }
    }
}