using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace StackErp.App
{
public static class HttpRequestExtensions
{
    private const string RequestedWithHeader = "X-Requested-With";
    private const string XmlHttpRequest = "XMLHttpRequest";

    public static bool IsAjaxRequest(this HttpRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException("request");
        }

        if (request.Headers != null)
        {
            return request.Headers[RequestedWithHeader] == XmlHttpRequest;
        }

        return false;
    }
}
}