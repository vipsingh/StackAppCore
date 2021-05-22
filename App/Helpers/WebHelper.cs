using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace StackErp.App
{
    public interface IWebHelper {
        bool IsRequestAvailable();
    }
    public class WebHelper: IWebHelper {
        public  WebHelper() {

        }

         public virtual bool IsRequestAvailable()
        {
            // if (_httpContextAccessor?.HttpContext == null)
            //     return false;

            // try
            // {
            //     if (_httpContextAccessor.HttpContext.Request == null)
            //         return false;
            // }
            // catch (Exception)
            // {
            //     return false;
            // }

            return true;
        }
    }
}