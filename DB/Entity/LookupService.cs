using System;
using System.Collections.Generic;
using Dapper;
using StackErp.Model;

namespace StackErp.DB.Entity
{
    public class LookupService
    {
        private static string LOOKUPDATA_QRY = "select dataid as Value, datatext as Text from lookup_master where id=@id {0};";
        public static IEnumerable<DbObject> GetLookupData(int lookupId)
        {
             var d = DBService.Query(String.Format(LOOKUPDATA_QRY, ""), new {id = lookupId });

             return d;
        }

        public static IEnumerable<DbObject> GetLookupData(int lookupId, List<int> value)
        {            
             var d = DBService.Query(String.Format(LOOKUPDATA_QRY, "and dataid = ANY(@value)"), new {id = lookupId, value });

             return d;
        }
        
    }
}
