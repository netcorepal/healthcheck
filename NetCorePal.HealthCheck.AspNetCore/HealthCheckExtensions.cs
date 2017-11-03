using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// 
    /// </summary>
    public static class HealthCheckExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="url"></param>
        /// <param name="apiKey">访问该接口的apiKey，如果不设置，则仅限本机访问，非法访问都将得到404错误</param>
        public static void UseHealthCheck(this IApplicationBuilder app, string url = "/healthcheck", string apiKey = null)
        {
            app.Map(url, p =>
            {
                p.Run(async context =>
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
                        //如果不是本地ip，则禁止请求
                    }

                    if (badVisitor)
                    {
                        context.Response.StatusCode = 404;
                        return;
                    }

                    var r = await HealthCheckerManager.Manager.CheckAllAsync();
                    if (r.Any(c => !c.IsHealthy))
                    {
                        context.Response.StatusCode = 500;
                    }
                    await context.Response.WriteAsync(r.ToHtml());
                });

            });
        }
    }
}