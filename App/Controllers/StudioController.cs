using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using StackErp.App.Models;
using StackErp.Model;
using StackErp.Model.Layout;
using StackErp.UI.Controllers;
using StackErp.UI.View.DataList;
using StackErp.UI.View.Desginer;
using StackErp.UI.View.Studio;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.App.Controllers
{
    public class StudioController: BaseController
    {
        public StudioController(ILogger<FilterController> logger,IOptions<AppKeySetting> appSettings): base(logger,appSettings)
        {

        }
        public IActionResult Index()
        {
            var context = new DeskPageContext(this.WebAppContext, EntityCode.EntityMaster, this.RequestQuery);
            context.Build();

            var builder = new UI.View.PageBuilder.EntityPageBuilder();            

            return CreatePageResult(builder.CreateDeskPage(context));
        }

        public IActionResult Studio(string id = null)
        {
            if (!string.IsNullOrEmpty(id))
            {
                this.RequestQuery.EntityId = EntityCode.Get(id);
            }

            var page = new StudioPage(WebAppContext);

            return CreatePageResult(page.GetPage(this.RequestQuery));
        }

        public IActionResult EntityList()
        {
            this.RequestQuery.EntityId = EntityCode.EntityMaster;
            var context = new DeskPageContext(this.WebAppContext, EntityCode.EntityMaster, this.RequestQuery);
            context.Build();

            var widgetContext = WidgetContext.BuildContext(context, "LayoutList");
            widgetContext.WidgetType = FormControlType.EntityListView;

            var widget = new ViewModel.FormWidget.EntityListWidget(widgetContext);
            widget.OnCompile();
            widget.SetValue(null);
            context.AddControl(widget);

            var page = new ViewPage(context);

            page.Actions = new InvariantDictionary<Model.Form.ActionInfo>();
            var qs = new RequestQueryString() { EntityId = EntityCode.EntityMaster };
            page.Actions.Add("BTN_NEW", new Model.Form.ActionInfo(AppLinkProvider.NEW_ENTITY_URL, qs) { Title = "New Entity", LinkTarget = "POPUP" });

            return CreatePageResult(page);
        }

        #region page designer

        public IActionResult LayoutDesigner() {
            var desinerPage = LayoutDesignerBuilder.BuildPage(WebAppContext, RequestQuery);

            return CreatePageResult(desinerPage);
        }        

        [HttpPost]
        public IActionResult SaveDesigner([FromBody]JObject data) 
        {
            if (data != null) {                
                var res = LayoutDesignerBuilder.SaveLayout(WebAppContext, RequestQuery, data);

                return CreateResult(res);
            }            

            return CreateResult(new ErrorResponse("Invalid data."));
        }


        #endregion
        // public IActionResult GetEntityDefData()
        // {

        // }
    }
}

/*
 * create entity - 
 */