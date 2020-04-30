using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackErp.Model;
using StackErp.ViewModel.Model;

namespace StackErp.UI.Controllers
{
    public class BaseController : Controller
    {
        private readonly ILogger<BaseController> _logger;    
        public StackAppContext StackAppContext {get;}
        public RequestQueryString RequestQuery {protected set; get;}
        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;    
            StackAppContext = new StackAppContext();
            StackAppContext.Init();
                        
        }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            RequestQuery = GetQuery(); 
                
            StackAppContext.RequestQuery = RequestQuery;
        }

        protected RequestQueryString GetQuery()
        {
            var q = new RequestQueryString();
            // if (Request.Query.ContainsKey("entity"))
            // {
            //     q.EntityId = EntityCode.Get(Request.Query["entity"].ToString());
            // }
            // if (Request.Query.ContainsKey("itemid"))
            // {
            //     q.ItemId = Convert.ToInt32(Request.Query["itemid"]);
            // }

            if (Request.Query.ContainsKey("q"))
            {
                q.Load(Request.Query["q"].ToString());
            } else if (Request.Query.ContainsKey("qx"))
            {
                q.LoadNonEncrypt(Request.Query["qx"].ToString());
            }

            return q;
        }

        protected IActionResult CreateResult(object page)
        {
            var result = new ActionResponse(page);

            return Json(result);
        }
        protected IActionResult CreatePageResult(object page)
        {
            var result = new PageResponse(page);

            return Json(result);
        }

        public IActionResult SPAApp()
        {
             var page = new App.PageLayout(this.StackAppContext);
            var m = page.Build();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(m);

            ViewBag.OrignalUrl = Request.Path.Value + (this.Request.QueryString != null ? this.Request.QueryString.ToString(): "");
            ViewBag.PageData = json;
            ViewBag.Host = Request.Scheme + "://" + Request.Host.ToUriComponent();

            return View("_App", m);
        }   
    }
}