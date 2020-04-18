using System;
using System.Collections.Generic;
using StackErp.Model;

namespace StackErp.DB.Entity
{
    public class LookupService
    {
        private static string LOOKUPDATA_QRY = "select dataid as Value, datatext as Text from lookup_master where id=@id";
        private static string LOOKUPOPTION_QRY = "select dataid as Value, datatext as Text from lookup_master where id=@id and dataid=@value";
        public static IEnumerable<DbObject> GetLookupData(int lookupId)
        {
             var d = DBService.Query(LOOKUPDATA_QRY, new {id = lookupId });

             return d;
        }

        public static DbObject GetLookupOption(int lookupId, int value)
        {
             var d = DBService.Single(LOOKUPOPTION_QRY, new {id = lookupId, value = value });

             return d;
        }
        
    }
}
