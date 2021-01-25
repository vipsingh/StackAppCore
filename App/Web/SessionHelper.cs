using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using StackErp.UI.View;
using StackErp.ViewModel.Services;

namespace StackErp.Web
{
    public class SessionHelper : ISessionHelper
    {
        public HttpContext HttpContext {get;}

        public ISession Session {get => HttpContext.Session;}
        public SessionHelper(HttpContext httpContext)
        {
            HttpContext = httpContext;
        }
        public void AddSession(string key, object value)
        {
            //Session.
        }

        public T GetSession<T>(string key)
        {
            throw new NotImplementedException();
        }
    }
}