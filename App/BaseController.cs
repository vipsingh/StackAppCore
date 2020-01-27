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
            if (Request.Query.ContainsKey("entity"))
            {
                q.EntityId = EntityCode.Get(Request.Query["entity"].ToString());
            }
            if (Request.Query.ContainsKey("itemid"))
            {
                q.ItemId = Convert.ToInt32(Request.Query["itemid"]);
            }

            if (Request.Query.ContainsKey("q"))
            {
                q.Load(Request.Query["q"].ToString());
            }

            return q;
        }

        protected IActionResult CreateResult(object page)
        {
            var result = new PageResponse(page);

            return Json(result);
        }

        static System.Text.Json.JsonSerializerOptions GetJsonOption()
        {
            var serializeOptions = new System.Text.Json.JsonSerializerOptions(){
                IgnoreNullValues = true,
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            };
            serializeOptions.Converters.Add(new App.Helpers.DynamicObjJsonConverter());

            return serializeOptions;
        }
    }
}