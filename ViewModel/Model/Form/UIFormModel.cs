using System;
using StackErp.Model;

namespace StackErp.ViewModel.Model
{
    public class UIFormModel
    {
        public EntityModelInfo EntityInfo {set; get;}
    }

    public class EntityModelInfo
    {
        public int ObjectId {private set; get;}
        public string EntityName {private set; get;}
        public DynamicObj Parameters {private set; get;}
        public EntityModelInfo(int objectId, string entityName)
        {
            ObjectId =objectId;
            EntityName=entityName;
        }
    }
}
