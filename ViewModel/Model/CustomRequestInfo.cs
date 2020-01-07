using System;
using StackErp.Model;

namespace StackErp.ViewModel.Model
{
    public class CustomRequestInfo
    {
        public int Mode {set;get;}
        public string FieldId {set;get;}
        public EditFormField FieldValue {set;get;}
        public EditFormModel Model {set;get;}
        public EntityModelInfo EntityInfo {set;get;}
        public DynamicObj Properties {set;get;}
        public RequestQueryString RequestQuery {set;get;}
        public object LinkInfo {set;get;}
    }
}