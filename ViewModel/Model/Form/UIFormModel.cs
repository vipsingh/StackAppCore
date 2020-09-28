using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using StackErp.Model;
using StackErp.Model.Form;

namespace StackErp.ViewModel.Model
{    
    public class UIFormModel
    {
        public DynamicObj Properties {set; get;}
        public ObjectModelInfo EntityInfo {set; get;}
        public InvariantDictionary<UIFormField> Widgets {set; get;}

        public object GetValue(string controlId) 
        {
            var val = this.Widgets[controlId];

            return val.Value;
        }
    }
    
    public class ObjectModelInfo: DynamicObj
    {
        public int ObjectId {set { this.Add("ObjectId", value, true); } get => this.Get("ObjectId", -1);}
        public EntityCode EntityId {set { this.Add("EntityId", value.Code, true); } get => this.Get("EntityId", 0);}
        public int ItemType {set { this.Add(ViewConstant.ItemType, value, true); } get => this.Get(ViewConstant.ItemType, 0);}
        public string OpenerQuery {set { this.Add(ViewConstant.OpenerQuery, value, true); } get => this.Get(ViewConstant.OpenerQuery, "");}
        public ObjectModelInfo()
        {

        }
        public ObjectModelInfo(int objectId, EntityCode entityId)
        {
            ObjectId =objectId;
            EntityId=entityId;
        }
    }

}
