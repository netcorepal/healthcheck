using System.Web;
using System.Web.Routing;
using System.Linq;
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
        /// <param name="routes">路由集合</param>
        /// <param name="url">路由地址，默认值“healthcheck”,不要以“/”开头</param>
        /// <param name="apiKey">访问该接口的apiKey，如果不设置，则仅限本机访问，非法访问都将得到404错误</param>
        public static void UseHealthCheck(this RouteCollection routes, string url = "healthcheck", string apiKey = null)
        {
            routes.Add(new HealthCheckManagerRoute(url, new HealthCheckRouteHandler(apiKey)));
        }
    }


    class HealthCheckManagerRoute : Route
    {
        public HealthCheckManagerRoute(string url, HealthCheckRouteHandler routeHandler) : base(url, routeHandler)
        {

        }
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }
    }

    class HealthCheckRouteHandler : IRouteHandler
    {
        AgentHttpHandler hander;
        public HealthCheckRouteHandler(string apiKey)
        {
            this.hander = new AgentHttpHandler(apiKey);
        }


        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return this.hander;
        }
    }


    class AgentHttpHandler : IHttpHandler
    {
        public AgentHttpHandler(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public string apiKey;
        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            bool badVisitor = false;
            if (apiKey != null)
            {
                if (context.Request.QueryString["apiKey"] != apiKey)
                {
                    badVisitor = true;
                }
            }
            else
            {
                if (!context.Request.IsLocal)
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
            context.Response.Write(html);
        }
    }
}
