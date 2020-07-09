using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using StackErp.Model;

namespace StackErp.App.Controllers
{
    // public class AppController : StackErp.UI.Controllers.BaseController
    // {
    //     public AppController(ILogger<AppController> logger, IOptions<AppKeySetting> appSettings): base(logger,appSettings)
    //     {
            
    //     }

    //     public IActionResult Index(string c, string  a)
    //     {  
    //         var page = new PageLayout(this.StackAppContext);
    //         var m = page.Build();
    //         var json = Newtonsoft.Json.JsonConvert.SerializeObject(m);

    //         ViewBag.OrignalUrl = $"{c}/{a}" + (this.Request.QueryString != null ? this.Request.QueryString.ToString(): "");
    //         ViewBag.PageData = json;
    //         ViewBag.Host = Request.Scheme + "://" + Request.Host.ToUriComponent();
    //         return View(m);
    //     }
    // }

    
}