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
    /// 健康检查组件扩展
    /// </summary>
    public static class HealthCheckExtensions
    {
        /// <summary>
        /// 注册HealthCheck中间件到指定url
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="url">Route，default:“/healthcheck”</param>
        /// <param name="apiKey">apiKey，all local if null, return http code 401 if apikey wrong</param>
        public static void UseHealthCheck(this IApplicationBuilder app, string url = "/healthcheck", string apiKey = null)
        {
            app.Map(url, p =>
            {
                p.Run(async context =>
                {
                    if (HttpMethods.Head.Equals(context.Request.Method, StringComparison.OrdinalIgnoreCase) || HttpMethods.Head.Equals(context.Request.Query["method"], StringComparison.OrdinalIgnoreCase))
                    {
                        return;
                    }
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
                        if (!context.Request.IsLocal())
                        {
                            badVisitor = true;
                        }
                    }

                    if (badVisitor)
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }
                     
                    HealthCheckResult[] r = await HealthCheckerManager.Manager.CheckAllAsync();
                    if (r.Any(c => !c.IsHealthy))
                    {
                        context.Response.StatusCode = 500;
                    }
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync(r.ToHtml());
                });

            });
        }
    }
}