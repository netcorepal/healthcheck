using Microsoft.Owin;
using Owin;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// 
    /// </summary>
    public static class HealthCheckExtenstions
    {
        /// <summary>
        /// Add HealthCheck Route
        /// </summary>
        /// <param name="app">IAppBuilder</param>
        /// <param name="url">Route，default “healthcheck”,do not use "/" start</param>
        /// <param name="apiKey">apiKey，all local if null, return http code 401 if apikey wrong</param>
        public static void UseHealthCheck(this IAppBuilder app, string url = "healthcheck", string apiKey = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("路由地址不能为空");
            }
            if (url.StartsWith("~", StringComparison.Ordinal) ||
                   url.StartsWith("/", StringComparison.Ordinal) ||
                   (url.IndexOf('?') != -1))
            {
                throw new ArgumentException("路由地址不能以“/”或“~”字符开头，并且不能包含“?”字符。");
            }
            app.Use<HealthyCheckMiddleware>(url, apiKey);
        }


        class HealthyCheckMiddleware : OwinMiddleware
        {
            public string url;
            public string apiKey;
            public HealthyCheckMiddleware(OwinMiddleware next,string url,string apiKey): base(next)
            {
                this.url = url;
                this.apiKey = apiKey;
            }

            public async override Task Invoke(IOwinContext context)
            {
                PathString tickPath = new PathString("/"+this.url);
                if (context.Request.Path.Equals(tickPath))
                {
                    bool badVisitor = false;
                    if (apiKey != null)
                    {
                        if (context.Request.Query["apiKey"] != apiKey)
                        {
                            badVisitor = true;
                        }
                    }
                    else
                    {
                        if (!((bool)context.Request.Environment["server.IsLocal"]))
                        {
                            badVisitor = true;
                        }
                    }

                    if (badVisitor)
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }


                    HealthCheckResult[] r;
                    if ("HEAD".Equals(context.Request.Method, System.StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }
                    else
                    {
                        r = HealthCheckerManager.Manager.CheckAllAsync().Result;
                    }
                    var html = r.ToHtml();
                    if (r.Any(p => !p.IsHealthy))
                    {
                        context.Response.StatusCode = 500;
                    }
                    context.Response.ContentType = "text/html;charset=UTF-8";
                    context.Response.Write(html);
                }
                else
                {
                    await Next.Invoke(context);
                }
               
            }
        }
    }
}
