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
        public IActionResult Studio()
        {
            var page = new StudioPage(StackAppContext);

            return CreatePageResult(page.GetPage(this.RequestQuery));
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

        #region page designer

        #endregion
        // public IActionResult GetEntityDefData()
        // {

        // }
    }
}
