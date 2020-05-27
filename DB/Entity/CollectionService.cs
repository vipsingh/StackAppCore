using System;
using System.Collections.Generic;
using Dapper;
using StackErp.Model;

namespace StackErp.DB.Entity
{
    public class CollectionService
    {
        private static string LOOKUPDATA_QRY = "select dataid as Value, datatext as Text from t_collection_master where id=@id {0};";
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
        
    }
}
