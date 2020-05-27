using System;
using System.Collections.Generic;
using StackErp.Model;
using StackErp.Model.Form;

namespace StackErp.App
{
    public class PageLayout
    {
        private StackAppContext _context;
        public PageLayout(StackAppContext context)
        {
            _context = context;
        }

        public AppPageModel Build()
        {
            var appPage = new  AppPageModel();
            BuildSidePanel(ref appPage);
            return appPage;
        }

        private void BuildSidePanel(ref AppPageModel page)
        {
            var menues = new List<ActionInfo>();

            StackErp.Core.Studio.StudioService ser = new Core.Studio.StudioService();
            var enyts = ser.GetAllEntities();
            
            var data = new Dictionary<int, string>();
            data.Add(111, "Customers");
            data.Add(112, "Product Category");
            data.Add(113, "Uom");
            data.Add(115, "Product");
            data.Add(131, "Area");
            data.Add(99, "Test");

            foreach(var e in data)
            {
                menues.Add(new ActionInfo("/entity/desk", new RequestQueryString(){ EntityId = e.Key }, "MENU_" + e.Key.ToString()){ Title = e.Value, ExecutionType = ActionExecutionType.Redirect });
            }
            page.SideMenues = menues;
        }
    }

    public class AppPageModel
    {
        public List<ActionInfo> SideMenues {set;get;}
    }
}
