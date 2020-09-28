using System;
using System.Collections.Generic;
using StackErp.Core.Entity;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.Helper
{
    public class CollectionDataHelper
    {
        public static List<SelectOption> GetCollectionData(WidgetContext context, List<int> values = null)
        {
            if (context.ControlDefinition != null && context.ControlDefinition.CollectionId > 0)
            {
                var collInfo = context.ControlDefinition.CollectionInfo;

                if (collInfo.SourceType == DataSourceType.Enum)
                {
                    return CollectionService.GetCollectionDataEnum(collInfo, values);
                }

                var data = CollectionService.GetCollectionData(context.ControlDefinition.CollectionId, values);
                var list = new List<SelectOption>();
                foreach(var o in data)
                {
                    if (o != null) {
                        var d = new SelectOption();
                        d.Add("Value", o.Get("Value", 0));
                        d.Add("Text", o.Get("Text", ""));
                        list.Add(d);
                    }
                }

                return list;
            }

            return null;
        } 
    }
}
