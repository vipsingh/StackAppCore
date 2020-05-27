using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StackErp.Model.Entity
{
    public abstract class RelationalField: BaseField 
    {
        
        public override void OnInit()
        {
            base.OnInit();
            ControlInfo.FieldAttribute.RefEntity = this.RefObject;
        }
    }
    public class OneToManyField: RelationalField
    {
        public string RefFieldName {get; private set;}
        public OneToManyField(): base() {
            Type = FieldType.OneToMany;
            BaseType = TypeCode.Object;
        }

        public void SetRelationShipInfo(EntityCode related, string field)
        {
            RefFieldName = field;
        }
    }
}