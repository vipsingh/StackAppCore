using System;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Form;

namespace StackErp.ViewModel.Model
{
    public class CustomRequestInfo
    {
        public int Mode {set;get;}
        public string WidgetId {set;get;}
        public object Value {set;get;}
        public FormControlType WidgetType {set;get;}
        public string Caption {set;get;}
        //public UIFormModel Model {set;get;}
        public ObjectModelInfo EntityInfo {set;get;}
        public DynamicObj Properties {set;get;}
        public RequestQueryString RequestQuery {set;get;}
        public DependencyDataContext DependencyContext  {set;get;}
        public DynamicObj Parameters {set;get;}
    }

    public class FieldRequestInfo: CustomRequestInfo
    {
        public object GetFieldValue(string widgetId) 
        {
            if (DependencyContext != null) return DependencyContext.GetWidgetValue(widgetId);

            return null;
        }
    }

    public class ListRequestinfo
    {
        public DataListRequestType RequestType {set;get;}
        public CustomRequestInfo RequestInfo {set;get;}
        public QueryRequest GridRequest  {set;get;}
    }
}