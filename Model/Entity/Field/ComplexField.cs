using System;
using System.Collections.Generic;

namespace StackErp.Model.Entity
{
    public class FilterField: LongTextField
    {
        public FilterField(): base() {
            Type = FieldType.FilterField;
        }

        public override void OnInit()
        {
            base.OnInit();
            ControlInfo.FieldAttribute.RefEntity = EntityCode.EntitySchema;
            BuildPickerDataSource();
        }

        private void BuildPickerDataSource()
        {
            var refEntity = this.Entity.GetEntity(EntityCode.EntitySchema);
            this.ControlInfo.DataSource = new FieldDataSource {
                Type = DataSourceType.Entity,
                Entity = EntityCode.EntitySchema,
                //Domain = "[[entityid,0,1]]" //this.Domain
            };
        }
    }
}
