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

            foreach(var e in enyts)
            {
                menues.Add(new ActionInfo("/entity/desk", new RequestQueryString(){ EntityId = e.Get("ID", 0) }, e.Get("ID", "")){ Title = e.Get("Name", ""), ExecutionType = ActionExecutionType.Redirect });
            }
            page.SideMenues = menues;
        }
    }

    public class AppPageModel
    {
        public List<ActionInfo> SideMenues {set;get;}
    }
}
