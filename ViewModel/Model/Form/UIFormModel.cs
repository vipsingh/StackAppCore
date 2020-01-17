using System;
using System.Collections.Generic;
using StackErp.Model;

namespace StackErp.ViewModel.Model
{
    public class UIFormModel
    {
        public DynamicObj Properties {set; get;}
        public EntityModelInfo EntityInfo {set; get;}
        public Dictionary<string, UIFormField> Fields {set; get;}

        public object GetValue(string controlId) 
        {
            var val = this.Fields[controlId];            

            return val.Value;
        }
    }

    public class UIFormField
    {
        public string ControlId {set; get;}
        public FormControlType WidgetType {set; get;}
        public DynamicObj Properties {set; get;}
        public object Value {set; get;}
        public string ErrorMessage {set; get;}
    }
    
    public class EntityModelInfo
    {
        public int ObjectId {set; get;}
        public EntityCode EntityId {set; get;}
        public DynamicObj Parameters {set; get;}
        public EntityModelInfo()
        {

        }
        public EntityModelInfo(int objectId, EntityCode entityId)
        {
            ObjectId =objectId;
            EntityId=entityId;
        }
    }
}
