using System;
using System.Collections.Generic;
using StackErp.Model;
using System.Linq;

namespace StackErp.Core.Entity
{
    public class LookupService
    {
        public static IEnumerable<DbObject> GetLookupData(int lookupId, List<int> values = null)
        {
            if (values != null) {
                return StackErp.DB.Entity.LookupService.GetLookupData(lookupId, values);
            }
            
            return StackErp.DB.Entity.LookupService.GetLookupData(lookupId);
        }

    }
}
