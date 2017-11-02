#if NET45
using System.Web;
using System.Web.Routing;
using System.Linq;
namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// 
    /// </summary>
    public static class RouteCollectionExtenstions
    {
        /// <summary>
        /// 注册HealthCheck路由
        /// </summary>
        /// <param name="routes">路由集合</param>
        /// <param name="url"></param>
        public static void UseHealthCheck(this RouteCollection routes, string url)
        {
            routes.Add(new HealthCheckManagerRoute(url, new HealthCheckRouteHandler()));
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
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return AgentHttpHandler.hander;
        }
    }


    class AgentHttpHandler : IHttpHandler
    {
        public static AgentHttpHandler hander = new AgentHttpHandler();

        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            var r = HealthCheckerManager.Manager.CheckAllAsync().Result;
            var html = r.ToHtml();
            if(r.Any(p=>!p.IsHealthy))
            {
                context.Response.StatusCode = 500;
            }
            context.Response.Write(html);
        }
    }
}

#endif
