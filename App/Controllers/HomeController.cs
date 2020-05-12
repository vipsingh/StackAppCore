using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackErp.App.Models;
using StackErp.Model;

namespace StackErp.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

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
