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
                var op = StackErp.DB.Entity.LookupService.GetLookupOption(lookupId, values[0]);
                return new List<DbObject>() { op };
            }
             return StackErp.DB.Entity.LookupService.GetLookupData(lookupId);
        }

    }
}
