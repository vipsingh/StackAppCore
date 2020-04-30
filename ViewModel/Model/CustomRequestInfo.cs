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
        public UIFormField Value {set;get;}
        public FormControlType WidgetType {set;get;}
        public string Caption {set;get;}
        public UIFormModel Model {set;get;}
        public ObjectModelInfo ModelInfo {set;get;}
        public DynamicObj Properties {set;get;}
        public RequestQueryString RequestQuery {set;get;}
        public object LinkInfo {set;get;}
    }

    public class ListRequestinfo
    {
        public DataListRequestType RequestType {set;get;}
        public CustomRequestInfo RequestInfo {set;get;}
        public QueryRequest GridRequest  {set;get;}
    }
}