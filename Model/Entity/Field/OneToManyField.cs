using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StackErp.Model.Entity
{    
    public class OneToManyField: RelationalField
    {
        public string RefFieldName {get; private set;}
        public OneToManyField(): base() {
            Type = FieldType.OneToMany;
            BaseType = TypeCode.Object;
            IsArrayData = true;
            IsLazyLoad = true;
        }

        public void SetRelationShipInfo(EntityCode related, string field)
        {
            RefFieldName = field;
        }
    }

    public class OneToOneField: RelationalField
    {
        public string RefFieldName {get; private set;}
        public OneToOneField(): base() {
            Type = FieldType.OneToOne;
            BaseType = TypeCode.Object;
            IsLazyLoad = true;
        }

        public void SetRelationShipInfo(EntityCode related, string field)
        {
            RefFieldName = field;
        }
    }
}