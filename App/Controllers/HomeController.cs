using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackErp.App.Models;
using StackErp.Core;
using StackErp.Model;
using StackErp.StackScript;

namespace StackErp.App.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        StackAppContext appContext;
        public HomeController(ILogger<HomeController> logger, IOptions<AppKeySetting> appSettings)
        {
            _logger = logger;
            ScriptFunctions.RegisterFunction("GetEntity", GetEntity);

            appContext = new StackAppContext();
            appContext.Init(appSettings.Value); 
        }

        public static Function GetEntity => new Function((arguments) =>
        {
            var arg1 = arguments.Get(0);
            
            return EntityMetaData.Get((int)arg1);
        });

        public IActionResult Index()
        {
            //var d = StackErp.Core.EntityMetaData.Get(EntityCode.Get("UserMaster")).GetSingle(1);
            return View();
        }

        [HttpPost]
        public IActionResult StackScript([FromBody] string scr)
        {
            new StackErp.StackScript.StackScriptExecuter().ExecuteScript(scr);

            return Content("executed");
        }

        [HttpPost]
        public IActionResult StackScriptFunction([FromQuery]int id, [FromBody] string scr)
        {
            var d = StackErp.Core.EntityMetaData.Get(99).GetSingle(id);
            
            var args = new Dictionary<string, object>();
            args.Add("model", d);

            new StackErp.StackScript.StackScriptExecuter().ExecuteFunction(appContext, scr, args);

            return Content("executed");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult QueryParser()
        {
            var q = new RequestQueryString();
            if (Request.Query.ContainsKey("q"))
            {
                q.Load(Request.Query["q"].ToString());
            }

            return Json(q);       
        }

        [HttpPost]
        public IActionResult TempX([FromBody] string p)
        {
            var fx = Model.Entity.FilterExpression.BuildFromJson(EntityCode.User, p);

            return Content(fx.ToJSONFormat());
        }
    }
}
