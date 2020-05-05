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
            this.ControlInfo.DataSource = new PickerDataSource {
                Type = "ENTITY",
                Entity = EntityCode.EntitySchema,
                Fields = new List<string>(){this.Entity.TextField},
                IdField = this.Entity.IDField,
                SortOnField = "",
                Domain = "[[entityid,0,1]]" //this.Domain
            };
        }
    }
}
