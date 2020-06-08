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
using StackErp.UI.View.DataList;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.App.Controllers
{
    public class AuthController: Controller
    {
        private readonly ILogger<AuthController> _logger;
        private AppKeySetting _appKeySetting;

        public AuthController(ILogger<AuthController> logger, IOptions<AppKeySetting> appSettings)
        {
            _logger = logger;
            _appKeySetting = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            var service  = new AuthService();
            var token = service.Authenticate(_appKeySetting, model.Username, model.Password);

            if (token == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Json(new { token = token });
        }

        [Authorize]
        public IActionResult Logout()
        {
            
            return Json(new { ok = true });
        }
    }
}