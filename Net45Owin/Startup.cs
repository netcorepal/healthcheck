using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using NetCorePal.HealthCheck;

[assembly: OwinStartup(typeof(Net45Owin.Startup))]

namespace Net45Owin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 有关如何配置应用程序的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkID=316888
            app.UseHealthCheck();
        }
    }
}
