using System;
using StackErp.Model;
using StackErp.Model.DataList;

namespace StackErp.ViewModel.Model
{
    public class CustomRequestInfo
    {
        public int Mode {set;get;}
        public string FieldId {set;get;}
        public UIFormField FieldValue {set;get;}
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