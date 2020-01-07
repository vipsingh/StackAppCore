using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace StackErp.App.Controllers
{
    public class AppController : BaseController
    {
        public AppController(ILogger<AppController> logger): base(logger)
        {

        }
    }
}