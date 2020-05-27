using System;
using System.Collections.Generic;
using StackErp.Model;
using System.Linq;

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

    }
}
