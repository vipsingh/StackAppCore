using System;
using System.Collections.Generic;
using System.Linq;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Form;

namespace StackErp.ViewModel.Model
{
    public class FormRule
    {
        public int Id {set;get;}
        public string Type {set;get;}
        public FormFilterExpression Criteria {set;get;} 
        public List<string> Fields {set;get;} 
    }

    public class WidgetDependencyInfo
    {
        public List<ParentDependencyInfo> Parents {set;get;}
        public List<ChildDependencyInfo> Children {set;get;}

        public void AddChild(ChildDependencyInfo child)
        {
            if (this.Children == null)
                this.Children =  new List<ChildDependencyInfo>();
                
            if (this.Children.Where(x => x.WidgetId == child.WidgetId).Count() == 0)
                this.Children.Add(child);
        }
    }

    public class ParentDependencyInfo
    {
        public string WidgetId {set;get;}
        public string Type {set;get;}
        public bool IsRequired {set;get;}
    }
    public class ChildDependencyInfo
    {
        public string WidgetId {set;get;}
        public bool Clear {set;get;}
    }

    public class DependencyDataContext
    {
        public WidgetDependencyInfo Dependency { set;get; }
        public InvariantDictionary<UIFormField> RefData {set; get;}

        public UIFormField GetWidgetData(string key)
        {
            if (RefData == null) return null;

            return RefData[key];
        }

        public object GetWidgetValue(string key)
        {
            var d = GetWidgetData(key);
            if (d != null) return d.Value;

            return null;
        }
    }
}
