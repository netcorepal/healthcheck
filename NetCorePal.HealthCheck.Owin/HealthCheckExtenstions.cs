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
        /// 注册HealthCheck路由
        /// </summary>
        /// <param name="app">Owin 配置</param>
        /// <param name="url">路由地址，默认值“healthcheck”,不要以“/”开头</param>
        /// <param name="apiKey">访问该接口的apiKey，如果不设置，则仅限本机访问，非法访问都将得到404错误</param>
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


                    var r = HealthCheckerManager.Manager.CheckAllAsync().Result;
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
