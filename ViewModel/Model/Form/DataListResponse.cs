using System;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Form;
using StackErp.ViewModel.FormWidget;

namespace StackErp.ViewModel.Model
{
    public class DataListResponse
    {
        public Dictionary<string, BaseWidget> Fields  {set; get;} 
        public DynamicObj Properties { get; }
        public object Data {set; get;} 
        public string RenderMode{set;get;}
        public string IdColumn {set;get;}
        public object Layout{set;get;}
        public Dictionary<string, ActionInfo> ActionButtons {get;}
    }
}