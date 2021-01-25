using System;
using System.Collections.Generic;

namespace StackErp.UI.View
{
    public sealed class PageNavigationHelper 
    {
        // private Stack<NavigationInfo> NavigationHistory
        // {
        //     get
        //     {
        //         return (Stack<NavigationInfo>)HttpContext.Current.Session["_navigationHistory"];
        //     }
        //     set
        //     {
        //         if (value == null)
        //             HttpContext.Current.Session.Remove("_navigationHistory");
        //         else
        //             HttpContext.Current.Session["_navigationHistory"] = value;
        //     }
        // }

        // public static bool IsAjaxRequest(HttpRequest request)
        // {
        //     if (request == null)
        //         throw new ArgumentNullException("request");
        //     if (request["X-Requested-With"] == "XMLHttpRequest")
        //         return true;
        //     if (request.Headers != null)
        //         return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        //     return false;
        // }
    }

    public class NavigationInfo 
    {

    }
}