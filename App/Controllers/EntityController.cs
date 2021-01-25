using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackErp.Model;
using StackErp.UI.View.PageAction;
using StackErp.UI.View.PageBuilder;
using StackErp.UI.View.DataList;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;
using StackErp.UI.Controllers;

namespace StackErp.App.Controllers
{
    public class EntityController : StackErp.UI.Controllers.BaseController
    {
        public EntityController(ILogger<EntityController> logger, IOptions<AppKeySetting> appSettings): base(logger,appSettings)
        {
            logger.LogDebug(1, "NLog injected into EntityController");
        }

        public IActionResult Index()
        { 
            return RedirectToAction("Desk", new { entityName = "UserMaster" });
        }
        public IActionResult New()
        {            

            var context = new EditFormContext(this.WebAppContext, this.RequestQuery.EntityId, this.RequestQuery);
            context.Build();

            var builder = new EntityPageBuilder();
            var page = builder.CreateNewPage(context);

            return CreatePageResult(page);
        }

        public IActionResult Edit()
        {
            var context = new EditFormContext(this.WebAppContext, this.RequestQuery.EntityId, this.RequestQuery);
            context.Build();

            var builder = new EntityPageBuilder();
            var page = builder.CreateEditPage(context);

            return CreatePageResult(page);
        }

        public IActionResult Detail()
        {
            var context = new DetailFormContext(this.WebAppContext, this.RequestQuery.EntityId, this.RequestQuery);
            context.Build();

            var builder = new EntityPageBuilder();
            var page = builder.CreateDetailPage(context);

            return CreatePageResult(page);
        }

        [HttpPost]
        public IActionResult Save([FromBody]UIFormModel model)
        {
            var s = ModelState;
            var pageAction = new EntityPageAction(this.WebAppContext);
            var actionRes = pageAction.GetSaveAction(this.RequestQuery, model);

            // var ent = Core.EntityMetaData.Get(EntityCode.UserRole);
            // var m = ent.GetDefault();
            
            // m.SetValue("Name", "Role XX");
            // ent.Save(m);
            
            return CreateResult(actionRes);
        }

        [HttpPost]
        public IActionResult List([FromBody] ListRequestinfo request)
        {
            DataListContext context = null;
            if (!string.IsNullOrEmpty(this.RequestQuery.RelationField))
            {
                context = new RelatedEntityListContext(this.WebAppContext, RequestQuery, request);
            }
            else
                context = new DataListContext(this.WebAppContext, RequestQuery, request);
            var builder = new EntityListBuilder();
            builder.Build(context);
            var res = builder.GetResponse(context);
            
            return CreateResult(res);
        }

        public IActionResult Desk(string id)
        {
            if (!string.IsNullOrEmpty(id)) {
                this.RequestQuery.EntityId = EntityCode.Get(id);
            }

            var context = new DeskPageContext(this.WebAppContext, this.RequestQuery.EntityId, this.RequestQuery);
            context.Build();

            var builder = new EntityPageBuilder();
            var page = builder.CreateDeskPage(context);

            return CreatePageResult(page);
        }

        [HttpPost]
        public IActionResult ExecFunction([FromBody]CustomRequestInfo requestInfo)
        {
            
            return null;
        }

        public IActionResult Testx([FromBody]CustomRequestInfo requestInfo)
        {
            var ent = Core.EntityMetaData.Get(EntityCode.User);
            var m = ent.GetDefault(WebAppContext);
            
            m.SetValue("SubmitAmount", 200);
            //m.ResolveComputedFields();

            return Content(m.GetValue("TotalAmount").ToString());
        }
    }
}