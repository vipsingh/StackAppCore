using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using StackErp.ViewModel.Services;

namespace StackErp.Web
{
    public class CacheHelper: ICacheHelper
    {
        public void Insert(string key, object value, double mins)
        {
            //HttpContext

            if (value == null || string.IsNullOrWhiteSpace(key) || mins <=0)
                return;

            // HttpContext.Current.Cache.Insert(
            //     key,
            //     o,
            //     null,
            //     System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(mins)
            //     );
        }
    }
}   