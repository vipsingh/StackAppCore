using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
using StackErp.Web;

namespace StackErp.App.Controllers
{
    public class AuthController: Controller
    {
        private readonly ILogger<AuthController> _logger;
        private AppKeySetting _appKeySetting;
        private AuthService _authService;

        public AuthController(ILogger<AuthController> logger, IOptions<AppKeySetting> appSettings)
        {
            _logger = logger;
            _appKeySetting = appSettings.Value;
            _authService  = new AuthService();
        }
        
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {            
            int userId;
            var claims = _authService.Authenticate(_appKeySetting, model.Username, model.Password, out userId);

            if (claims == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            };

            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claims),
                authProperties);

            _authService.PrepareUserSession(userId, HttpContext);

            return Json(new ActionResponse(new { token = 1 }));
        }

        [Authorize]
        public IActionResult Logout()
        {
            _authService.Logout(HttpContext);

            return RedirectToAction("Login", new { isLogout = true });
        }
    }
}