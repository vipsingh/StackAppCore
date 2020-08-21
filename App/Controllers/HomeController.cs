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
using StackErp.ViewModel.Model;

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

            appContext = new StackAppContext();
            appContext.Init(appSettings.Value); 
        }
        
        public IActionResult Index()
        {
            //var d = StackErp.Core.EntityMetaData.Get(EntityCode.Get("UserMaster")).GetSingle(1);
            return View();
        }

        [HttpPost]
        public IActionResult StackScript([FromBody] string scr)
        {
            StackErp.StackScript.StackScriptParser.Parse(scr);

            return Content("executed");
        }

        [HttpPost]
        public IActionResult StackScriptFunction([FromQuery]int id, [FromBody] string scr)
        {
            var args = new Dictionary<string, object>();

            // FieldRequestInfo requestInfo = new  FieldRequestInfo();
            // requestInfo.Value = id;
            // args.Add("input", requestInfo);

            // FieldActionResponse output = new FieldActionResponse(null);
            // var sts = new StackErp.StackScript.StackScriptExecuter().ExecuteFunction(appContext, scr, args, output);

            /////////////////////////////////////////////////////////////////////

            var d = StackErp.Core.EntityMetaData.Get(99).GetSingle(id);
            args.Add("model", d);

            DynamicObj output = new DynamicObj();
            var sts = new StackErp.StackScript.StackScriptExecuter().ExecuteFunction(appContext, scr, args, output);

            return Json(new {sts, output});
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
