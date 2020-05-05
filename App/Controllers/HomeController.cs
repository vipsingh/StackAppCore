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

        [lizzie.Bind(Name = "write")]
        object WriteLine(lizzie.Binder<HomeController> binder, lizzie.Arguments arguments)
        {
            Console.WriteLine(arguments.Get(0));
            return null;
        }
        [lizzie.Bind(Name = "ev")]
        object Eval(lizzie.Binder<HomeController> binder, lizzie.Arguments arguments)
        {
            var str = arguments.Get(0);
            return null;
        }

        public IActionResult lizz()
        {

            /*
            should be parse (2 + 3) as +(2,3)
            */
            string code = @"
                var(@foo, 5)
                ev(foo + 11)
                
            ";

            var lambda = lizzie.LambdaCompiler.Compile(new HomeController(null), code);
            var result = lambda();

            return Json(new { x = result });
        }

        [HttpPost]
        public IActionResult Esprima([FromBody] string scr)
        {
            return Content(StackErp.StackScript.StackScriptParser.Parse(scr));
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
    }
}
