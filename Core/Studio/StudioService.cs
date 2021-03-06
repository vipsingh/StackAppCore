using System;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Core;

namespace StackErp.Core.Studio
{
    public class StudioService
    {        
        public List<DynamicObj> GetEntityListData()
        {
            var ents = new List<DynamicObj>();

            foreach(var entK in EntityMetaData.entities)
            {
                var ent = entK.Value;
                if (ent.EntityType != EntityType.CoreEntity) continue;                

                var o =  new DynamicObj();
                o.Add("Module", ent.AppModule);
                o.Add("Name", ent.Name);
                o.Add("Text", ent.Name);
                o.Add("ID", ent.EntityId.Code);

                ents.Add(o);
            }

            return ents;
        }

        public void GetEntity(int entityId)
        {
            
        }

        public void SaveEntity()
        {

        }
    }
}
