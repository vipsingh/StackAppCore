using System;

namespace StackErp.Model
{
    public enum ListingType {
        EntityList = 1,
        RelatedEntityList = 2,
        Report = 3        
    }

    public enum DataListRequestType
    {
        SchemaWithData = 0,
        OnlyData = 1,
        OnlySchema = 2
    }
}
