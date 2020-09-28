using System;
using System.Collections.Generic;
using StackErp.Model;
using System.Linq;
using StackErp.Model.Entity;
using System.Reflection;

namespace StackErp.Core.Entity
{
    public class CollectionService
    {
        public static IEnumerable<DbObject> GetCollectionData(int lookupId, List<int> values = null)
        {
            if (values != null) {
                return StackErp.DB.Entity.CollectionService.GetCollectionData(lookupId, values);
            }
            
            return StackErp.DB.Entity.CollectionService.GetCollectionData(lookupId);
        }

        public static List<SelectOption> GetCollectionDataEnum(CollectionInfo collectionInfo, List<int> values = null)
        {
           Type enumType =  Type.GetType(collectionInfo.SourceExp);
           FieldInfo[] fields = enumType.GetFields();
           var list = new List<SelectOption>();
           foreach (var field in fields) {
                if (field.Name.Equals("value__")) continue;         

                var v =  (int)field.GetRawConstantValue();
                if (values != null) {
                    if (!values.Contains(v)) {
                        continue;
                    }
                }

                var d = new SelectOption();
                d.Add("Value", v);
                d.Add("Text", field.Name);
                list.Add(d);      
            }            

            return list;
        }

        public static CollectionInfo GetCollectionInfo(int collectionId)
        {
            return StackErp.DB.Entity.CollectionService.GetCollectionInfo(collectionId);
        }

    }
}
