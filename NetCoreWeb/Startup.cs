using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCorePal.HealthCheck;
namespace NetCoreWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHealthCheck();
            HealthCheckerManager.Manager.Add("aaa", () =>
            {
                System.Threading.Thread.Sleep(2000);
                return new HealthCheckResult()
                {
                    IsHealthy = true,
                    Message = "slow",
                    Name = "aaa"
                };
            });
            HealthCheckerManager.Manager.AddHttpHealthChecker("baidu", "https://www.baidu.com");
            HealthCheckerManager.Manager.SetLogger(app.ApplicationServices.GetService<ILogger<HealthCheckerManager>>());
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
