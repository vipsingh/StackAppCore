using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.DB.Entity
{
    public class CollectionService
    {
        private static string LOOKUPDATA_QRY = "select dataid as Value, datatext as Text from t_collection_master where id=@id {0};";
        private static string COLLECTION_QRY = "select * from t_collection where id=@id;";
        public static IEnumerable<DbObject> GetCollectionData(int lookupId)
        {
             var d = DBService.Query(String.Format(LOOKUPDATA_QRY, ""), new {id = lookupId });

             return d;
        }

        public static IEnumerable<DbObject> GetCollectionData(int lookupId, List<int> value)
        {            
             var d = DBService.Query(String.Format(LOOKUPDATA_QRY, "and dataid = ANY(@value)"), new {id = lookupId, value });

             return d;
        }

        public static CollectionInfo GetCollectionInfo(int collectionId)
        {
             var d = DBService.Query(String.Format(COLLECTION_QRY, ""), new {id = collectionId });
             if (d.Count() > 0)
             {
                 var data = d.First();
                var coll = new CollectionInfo(data.Get("id", 0), data.Get("name", ""), 
                    data.Get("type", 0), data.Get("sourcetype", 0), 
                    data.Get("sourceexp", ""), data.Get("valuefield", "") , data.Get("textfield", ""));
                coll.MaxCount = data.Get("maxcount", 50);

                return coll;
             }
             
             return null;
        }
        
    }
}
