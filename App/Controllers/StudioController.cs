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
    [SPA]
    public class StudioController: Controller
    {
        public StudioController()
        {

        }
        public IActionResult Index()
        {            
            return View();
        }
    }
}
