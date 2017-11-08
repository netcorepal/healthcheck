using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NetCorePal.HealthCheck
{
    static class HttpRequestExtensions
    {
        public static bool IsLocal(this HttpRequest request)
        {
            var connection = request.HttpContext.Connection;
            if (connection.RemoteIpAddress != null)
            {
                return IPAddress.IsLoopback(connection.RemoteIpAddress);
            }
            else
            {
                return false;
            }
        }
    }
}
