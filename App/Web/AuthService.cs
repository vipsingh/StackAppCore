using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackErp.Core;
using StackErp.Core.Entity;
using StackErp.Model;
using StackErp.UI.View;

namespace StackErp.Web
{
    public class AuthService
    {
        public ClaimsIdentity Authenticate(AppKeySetting appKey, string username, string password, out int userId)
        {
            var userEntity = EntityMetaData.GetAs<UserDbEntity>(EntityCode.User);
            string email;
            userId = userEntity.AuthenticateUser(username, password, out email);

            if (userId <= 0) return null;

            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(ClaimTypes.Name, username),
                new System.Security.Claims.Claim(ClaimTypes.Sid, userId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            return claimsIdentity;
        }

        public void PrepareUserSession(int userId, HttpContext context)
        {
            var roles = UserRoleEntity.GetUserRoles(userId);
            var role  = roles.First();
            var userCntxt = new UserContext();
            userCntxt.UserId = userId;
            userCntxt.RoleId = role.Get("id", -1);   

            userCntxt.EntityAccessData = UserRoleEntity.GetEntityAccessData(userCntxt.MasterId, userCntxt.RoleId);            

            context.Session.SetString("USER_CONTEXT", JsonConvert.SerializeObject(userCntxt));
        }

        public void Logout(HttpContext context)
        {
            context.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            context.Session.Clear();
        }

        public WebAppContext GetAppContext(AppKeySetting appSettings, HttpContext context)
        {
            var sessionHelper = new SessionHelper(context);
            var stackAppContext = new WebAppContext(sessionHelper);
            stackAppContext.Init(appSettings); 
            var str = context.Session.GetString("USER_CONTEXT");

            if (!string.IsNullOrEmpty(str)){
                var userCntxt = JsonConvert.DeserializeObject<UserContext>(str);

                stackAppContext.UserInfo = userCntxt;
            }

            return stackAppContext;
        }
        // public string Authenticate1(AppKeySetting appKey, string username, string password)
        // {
        //     var userEntity = EntityMetaData.GetAs<UserDbEntity>(EntityCode.User);
        //     string email;
        //     var userId = userEntity.AuthenticateUser(username, password, out email);

        //     if (userId <= 0) return null;

        //     // authentication successful so generate jwt token
        //     var tokenHandler = new JwtSecurityTokenHandler();
        //     var key = Encoding.ASCII.GetBytes(appKey.JWTSecret);
        //     var tokenDescriptor = new SecurityTokenDescriptor
        //     {
        //         Subject = new ClaimsIdentity(new Claim[]
        //         {
        //             new System.Security.Claims.Claim(ClaimTypes.Email, email)
        //         }),
        //         Expires = DateTime.UtcNow.AddDays(7),
        //         SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //     };

        //     var token = tokenHandler.CreateToken(tokenDescriptor);
        //     return tokenHandler.WriteToken(token);
        // }
    }

}