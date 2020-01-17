using System;

namespace StackErp.Model.Entity
{
    public class LinkField: BaseField {
        public LinkField(): base() {
            Type = FieldType.ObjectLink;
            BaseType = BaseTypeCode.Int32;
        }
    }

    public class SelectField: BaseField {
        public SelectField(): base() {
            Type = FieldType.Select;
            BaseType = BaseTypeCode.Int32;
        }
    }
}