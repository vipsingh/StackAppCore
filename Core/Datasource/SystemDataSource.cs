using System;
using System.Collections.Generic;
using StackErp.Model.Entity;

namespace StackErp.Core.Datasource
{
    public class SystemDataSource
    {
        private static Dictionary<int, PickerDataSource> _PickerQuery;
        static SystemDataSource() {
            _PickerQuery = new  Dictionary<int, PickerDataSource>();
        }
    }
}
