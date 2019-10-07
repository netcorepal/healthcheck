using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// HttpHeadHealthChecker
    /// </summary>
    public class HttpHeadHealthChecker : IHealthChecker
    {
        readonly string url;

        readonly HttpClient client;
        /// <summary>
        /// 接入方需要支持head请求
        /// </summary>
        /// <param name="name">checker name</param>
        /// <param name="url">http url to check</param>
        public HttpHeadHealthChecker(string name, string url)
        {
            this.Name = name;
            this.url = url;
            this.client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(15)
            };
            this.client.DefaultRequestHeaders.Add("health-checker", "httpclient");
        }
        /// <summary>
        /// checker name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<HealthCheckResult> CheckAsync()
        {
            await client.SendAsync(new HttpRequestMessage
            {
                Method = new HttpMethod("HEAD"),
                RequestUri = new Uri(url)
            }).ConfigureAwait(false);

            return null;
        }
    }
}
