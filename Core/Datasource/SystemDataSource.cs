using System;
using System.Collections.Generic;
using StackErp.Model.Entity;

namespace StackErp.Core.Datasource
{
    public class SystemDataSource
    {
        private static Dictionary<int, FieldDataSource> _PickerQuery;
        static SystemDataSource() {
            _PickerQuery = new  Dictionary<int, FieldDataSource>();
        }
    }
}
