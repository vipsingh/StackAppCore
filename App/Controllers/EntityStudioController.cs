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
using StackErp.UI.View.Studio;

namespace StackErp.App.Controllers
{
    [SPA(Ignore = true)]
    public class EntityStudioController : BaseController
    {
        public EntityStudioController(ILogger<EntityController> logger, IOptions<AppKeySetting> appSettings) : base(logger, appSettings)
        {
            //logger.LogDebug(1, "NLog injected into EntityController");
        }
        public IActionResult Index(string a, string id = null)
        {
            ViewBag.Host = Request.Scheme + "://" + Request.Host.ToUriComponent();
            ViewBag.AppRoot = Url.Content("~/");

            return View();
        }
    }
}
