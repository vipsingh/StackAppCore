using System;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.Core.Datasource
{
    public class SystemDataSource
    {
        private static Dictionary<string, FieldDataSource> _PickerSource;
        static SystemDataSource() {
            _PickerSource = new  Dictionary<string, FieldDataSource>();
            AddSystemPickerSource();
        }

        private static void AddSystemPickerSource()
        {
            _PickerSource.Add("SYSTEM_2", new FieldDataSource() {
                Type = DataSourceType.Entity,
                Entity = EntityCode.EntitySchema,
                Domain = FilterExpression.BuildFromJson(EntityCode.EntitySchema, "[{\"entityid\": [0,\"@$params.EntityId\"]}]"),
                IncludeGlobalMasterId = true
            });
        }

        public static FieldDataSource GetPickerSource(string name)
        {
            return _PickerSource[name];
        }
    }
}
