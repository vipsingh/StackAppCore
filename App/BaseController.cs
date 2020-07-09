using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackErp.App.Controllers;
using StackErp.Model;
using StackErp.ViewModel.Model;

namespace StackErp.UI.Controllers
{
    [SPA]
    [Authorize]
    //[Secure]
    public class BaseController : Controller
    {
        private readonly ILogger<BaseController> _logger;    
        private AppKeySetting _appSetting;
        public StackAppContext StackAppContext {private set; get;}
        public RequestQueryString RequestQuery {protected set; get;}
        public BaseController(ILogger<BaseController> logger, IOptions<AppKeySetting> appSettings)
        {
            _logger = logger;    
            _appSetting = appSettings.Value;
        }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            if (HttpContext.Session.Keys.Count() == 0)
            {
                if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
                {
                    var cl = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Sid);
                    if (cl.Count() > 0)
                    {
                        (new Web.AuthService()).PrepareUserSession(int.Parse(cl.First().Value), HttpContext);

                        if (HttpContext.Session.IsAvailable)
                            StackAppContext = (new Web.AuthService()).GetAppContext(_appSetting, HttpContext);

                        RequestQuery = GetQuery(); 
                        
                        StackAppContext.RequestQuery = RequestQuery;
                    }
                    else 
                    {
                        (new Web.AuthService()).Logout(HttpContext);
                        context.Result = RedirectToAction("Login", "Auth", new { isLogout = true });
                    }
                }
                else
                {
                    (new Web.AuthService()).Logout(HttpContext);
                    context.Result = RedirectToAction("Login", "Auth", new { isLogout = true });
                }
            }
            else 
            {
                if (HttpContext.Session.IsAvailable)
                    StackAppContext = (new Web.AuthService()).GetAppContext(_appSetting, HttpContext);

                RequestQuery = GetQuery(); 
                    
                StackAppContext.RequestQuery = RequestQuery;
            }
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

        [AllowAnonymous]
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //new App.Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
            return Json(new ErrorResponse("Error"));
        } 
    }

    public class SPAAttribute : ActionFilterAttribute
    {
        public bool Ignore {set;get;}
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Ignore) 
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            StackErp.UI.Controllers.BaseController baseController = filterContext.Controller as StackErp.UI.Controllers.BaseController;
            var eqs = baseController.RequestQuery;

            if (filterContext.HttpContext.Request.Query["_ajax"] == "1")
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                filterContext.Result = baseController.SPAApp();
            }
        }
    }

    public class SecureAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context != null)
            {
                var userName = context.HttpContext.User.Identity;
            }
        }
    }
}