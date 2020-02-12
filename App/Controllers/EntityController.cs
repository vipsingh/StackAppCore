using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackErp.Model;
using StackErp.UI.View.PageAction;
using StackErp.UI.View.PageBuilder;
using StackErp.ViewModel.DataList;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.App.Controllers
{
    public class EntityController : StackErp.UI.Controllers.BaseController
    {
        public EntityController(ILogger<EntityController> logger): base(logger)
        {

        }

        public IActionResult New()
        {            

            var context = new EditFormContext(this.StackAppContext, this.RequestQuery.EntityId, this.RequestQuery);
            context.Build();

            var builder = new EntityPageBuilder();
            var page = builder.CreateNewPage(context);

            return CreatePageResult(page);
        }

        public IActionResult Edit()
        {
            var context = new EditFormContext(this.StackAppContext, this.RequestQuery.EntityId, this.RequestQuery);
            context.Build();

            var builder = new EntityPageBuilder();
            var page = builder.CreateEditPage(context);

            return CreatePageResult(page);
        }

        public IActionResult Detail()
        {
            var context = new DetailFormContext(this.StackAppContext, this.RequestQuery.EntityId, this.RequestQuery);
            context.Build();

            var builder = new EntityPageBuilder();
            var page = builder.CreateDetailPage(context);

            return CreatePageResult(page);
        }

        [HttpPost]
        public IActionResult Save([FromBody] UIFormModel model)
        {
            var s = ModelState;
            var pageAction = new EntityPageAction(this.StackAppContext);
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
            var context = new DataListContext(this.StackAppContext, request);
            var builder = new EntityListBuilder();
            builder.Build(context);
            var res = builder.GetResponse(context);
            
            return CreateResult(res);
        }

        public IActionResult Desk()
        {
            var context = new DeskPageContext(this.StackAppContext, this.RequestQuery.EntityId, this.RequestQuery);
            context.Build();

            var builder = new EntityPageBuilder();
            var page = builder.CreateDeskPage(context);

            return CreatePageResult(page);
        }
    }
}