using System;

namespace StackErp.Model.Entity
{
    public class ImageField: StringField
    {
        public ImageField(): base() {
            Type = FieldType.Image;
            BaseType = TypeCode.String;
        }

        public override object ResolveSetValue(object val, out bool isValid)
        {
            isValid = true;
            if (val is string)
                return val;
            else if (val is DynamicObj)
                return val;
            else
                return DataHelper.GetData(val, string.Empty);
        }
    }
}