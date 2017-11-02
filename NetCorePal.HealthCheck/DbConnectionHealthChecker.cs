using System;
using System.Threading.Tasks;
using System.Data.Common;
namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// 
    /// </summary>
    public class DbConnectionHealthChecker : IHealthChecker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="factory"></param>
        /// <param name="connectionString"></param>
        public DbConnectionHealthChecker(string name, DbProviderFactory factory, string connectionString)
        {
            this.factory = factory;
            this.connectionString = connectionString;
            this.Name = name;
        }


#if NET45
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="provider"></param>
        /// <param name="connectionString"></param>
        public DbConnectionHealthChecker(string name, string provider, string connectionString )
        {
            this.Name = name;
            this.connectionString = connectionString;
            this.factory = DbProviderFactories.GetFactory(provider);
        }
#endif
        DbProviderFactory factory;
        string connectionString;
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 
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
