using System;
using System.Threading.Tasks;
using System.Data.Common;
namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// 数据库连接检查器
    /// </summary>
    public class DbConnectionHealthChecker : IHealthChecker
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">检查器名称</param>
        /// <param name="factory">数据库提供程序工厂实例</param>
        /// <param name="connectionString">连接字符串</param>
        public DbConnectionHealthChecker(string name, DbProviderFactory factory, string connectionString)
        {
            this.factory = factory;
            this.connectionString = connectionString;
            this.Name = name;
        }


#if NET45
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">检查器名称</param>
        /// <param name="provider">提供程序名称，web.config system.data节DbProviderFactories中配置的invariant值</param>
        /// <param name="connectionString">连接字符串</param>
        public DbConnectionHealthChecker(string name, string provider, string connectionString)
        {
            this.Name = name;
            this.connectionString = connectionString;
            this.factory = DbProviderFactories.GetFactory(provider);
        }
#endif
        DbProviderFactory factory;
        string connectionString;
        /// <summary>
        /// 检查器名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 创建DbConnection并调用OpenAsync，检查连接是否可以打开
        /// </summary>
        /// <returns></returns>
        public virtual async Task<HealthCheckResult> CheckAsync()
        {
            using (var con = this.factory.CreateConnection())
            {
                con.ConnectionString = this.connectionString;
                await con.OpenAsync();
                return new HealthCheckResult { Name = this.Name, IsHealthy = true };
            }
        }


    }
}
