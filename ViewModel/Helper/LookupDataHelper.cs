using System;
using System.Collections.Generic;
using StackErp.Core.Entity;
using StackErp.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.Helper
{
    public class LookupDataHelper
    {
        public static List<SelectOption> GetLookupData(WidgetContext context, List<int> values = null)
        {
            if (context.ControlDefinition != null && context.ControlDefinition.LookupId > 0)
            {
                var data = LookupService.GetLookupData(context.ControlDefinition.LookupId, values);
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
