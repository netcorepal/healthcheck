using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.HealthCheck.DbCluster
{
    /// <summary>
    /// 面向.NetFramework数据库配置（集群版）的健康检查
    /// </summary>
    public class DbClusterHealthCheck : HealthChecker
    {
        private readonly DbProviderFactory connectionFactory;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">Checker 自定义名称</param>
        /// <param name="connectionFactory">rebbitmq 连接工厂对象</param>
        public DbClusterHealthCheck(string name, string connectionString, string provider = "MySql.Data.MySqlClient")
        {
            this.Name = name;
            this.connectionFactory = DbProviderFactories.GetFactory(provider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override HealthCheckResult Check()
        {
            return CheckAsync().Result;
        }

        public override async Task<HealthCheckResult> CheckAsync()
        {
            using (var connection = this.connectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                return new HealthCheckResult { Name = this.Name, IsHealthy = true };
            }
        }
    }
}
