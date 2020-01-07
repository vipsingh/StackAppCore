using System;
using System.Collections.Generic;
using StackErp.Model;

namespace StackErp.ViewModel.Model
{
    public class EditFormModel
    {
        public string EntityName {private set; get;}
        public DynamicObj Properties {set; get;}
        public EntityModelInfo EntityInfo {set; get;}
        public Dictionary<string, EditFormField> Fields {set; get;}

        public object GetValue(string controlId) 
        {
            var val = this.Fields[controlId];            

            return val.Value;
        }
    }

    public class EditFormField
    {
        public string ControlId {set; get;}
        public DynamicObj Properties {set; get;}
        public object Value {set; get;}
        public string ErrorMessage {set; get;}
    }
}