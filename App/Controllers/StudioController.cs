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
using StackErp.UI.Controllers;
using StackErp.UI.View.DataList;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.App.Controllers
{
    [SPA]
    public class StudioController: BaseController
    {
        public StudioController(ILogger<AppController> logger,IOptions<AppKeySetting> appSettings): base(logger,appSettings)
        {

        }
        public IActionResult Studio()
        {
            var page = new ViewPage();
            page.PageType = AppPageType.AppStudio;
            page.CurrentQuery = this.RequestQuery.ToQueryString();
            page.Actions = new InvariantDictionary<Model.Form.ActionInfo>();
            page.Actions.Add("BTN_NEW", new Model.Form.ActionInfo(AppLinkProvider.NEW_ENTITY_URL, this.RequestQuery){ Title = "New", LinkTarget="POPUP" });

            return CreatePageResult(page);
        }

        public IActionResult GetFieldList([FromBody] ListRequestinfo request)
        {
            var qs =  new RequestQueryString();
            qs.EntityId = EntityCode.EntitySchema;
            if (request.GridRequest == null)
            {
                request.GridRequest = new Model.DataList.QueryRequest();
                var filter = JObject.Parse("{\"$and\":[{\"entityid\":[0," + RequestQuery.EntityId.Code + "]}]}");
                request.GridRequest.DataFilter = filter;
            }

            var context = new DataListContext(this.StackAppContext, qs, request);
            var builder = new EntityListBuilder();
            builder.Build(context);
            var res = builder.GetResponse(context);
            
            return CreateResult(res);
        }

        // public IActionResult GetEntityDefData()
        // {

        // }
    }
}
