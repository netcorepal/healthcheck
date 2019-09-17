using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

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
        /// <param name="config">WebApi 配置</param>
        /// <param name="url">路由地址，默认值“healthcheck”,不要以“/”开头</param>
        /// <param name="apiKey">访问该接口的apiKey，如果不设置，则仅限本机访问，非法访问都将得到404错误</param>
        public static void UseHealthCheck(this HttpConfiguration config, string url = "healthcheck", string apiKey = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("路由地址不能为空");
            }
            config.Routes.MapHttpRoute(
                   name: "Healthy Check",
                   routeTemplate: url,
                   defaults: null,
                   constraints: null,
                   handler: new HealthyCheckHandler(apiKey)
               );
        }

        /// <summary>
        /// 从HttpRequestMessage中获取查询参数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetQueryString(this HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();
            if (queryStrings == null)
                return null;

            var match = queryStrings.FirstOrDefault(kv => string.Compare(kv.Key, key, true) == 0);
            if (string.IsNullOrEmpty(match.Value))
                return null;

            return match.Value;
        }

        class HealthyCheckHandler : DelegatingHandler
        {
            public string apiKey;
            public HealthyCheckHandler(string apiKey)
            {
                this.apiKey = apiKey;
            }
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage();
                if ("HEAD".Equals(request.Method.Method, StringComparison.OrdinalIgnoreCase) || "HEAD".Equals(request.GetQueryString("method"), StringComparison.OrdinalIgnoreCase))
                {
                    responseMessage.StatusCode = System.Net.HttpStatusCode.OK;
                    return Task.FromResult(responseMessage);
                }
                bool badVisitor = false;
                if (apiKey != null)
                {
                    if (request.GetQueryString("apiKey") != apiKey)
                    {
                        badVisitor = true;
                    }
                }
                else
                {
                    if (!request.IsLocal())
                    {
                        badVisitor = true;
                    }
                }

                if (badVisitor)
                {
                    responseMessage.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                }
                else
                {

                    HealthCheckResult[] r = HealthCheckerManager.Manager.CheckAllAsync().Result;
                    var html = r.ToHtml();
                    if (r.Any(p => !p.IsHealthy))
                    {
                        responseMessage.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    }
                    responseMessage.Content = new StringContent(html, Encoding.UTF8);
                    responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html")
                    {
                        CharSet = "utf-8"
                    };
                }
                return Task.FromResult(responseMessage);
            }
        }
    }
}
