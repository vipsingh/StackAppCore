using System;
using System.Collections.Generic;

namespace StackErp.Core.Datasource
{
    public class SystemDataSource
    {
        private static Dictionary<string, string> _PickerQuery;
        static SystemDataSource() {
            _PickerQuery = new  Dictionary<string, string>();
        }
    }
}
