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

        public IActionResult Studio()
        {
            var page = new StudioPage(WebAppContext);

            return CreatePageResult(page.GetPage(this.RequestQuery));
        }

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

        #region page designer

        #endregion
        // public IActionResult GetEntityDefData()
        // {

        // }
    }
}
