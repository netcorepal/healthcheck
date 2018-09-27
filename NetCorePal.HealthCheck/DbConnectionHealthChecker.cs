using System;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;
namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// DbConnection Health Checker 
    /// </summary>
    public class DbConnectionHealthChecker : IHealthChecker
    {
        readonly Func<IDbConnection> factory;

        /// <summary>
        /// DbConnectionHealthChecker
        /// </summary>
        /// <param name="name">Checker name</param>
        /// <param name="factory">DbProviderFactory</param>
        /// <param name="connectionString">connectionString</param>
        public DbConnectionHealthChecker(string name, DbProviderFactory factory, string connectionString) : this(name, () => FactoryImp(factory, connectionString))
        {
        }
        /// <summary>
        /// DbConnectionHealthChecker
        /// </summary>
        /// <param name="name">Checker name</param>
        /// <param name="factory">IDbConnection factory</param>
        public DbConnectionHealthChecker(string name, Func<IDbConnection> factory)
        {
            this.Name = name ?? throw new ArgumentNullException(name);
            this.factory = factory;
        }


#if NET45
        /// <summary>
        /// DbConnectionHealthChecker
        /// </summary>
        /// <param name="name">Checker name</param>
        /// <param name="provider">providerName，web.config system.data section DbProviderFactories invariant value</param>
        /// <param name="connectionString">连接字符串</param>
        public DbConnectionHealthChecker(string name, string provider, string connectionString) : this(name, DbProviderFactories.GetFactory(provider), connectionString)
        {
        }
#endif

        private static IDbConnection FactoryImp(DbProviderFactory factory, string connectionString)
        {
            var connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            return connection;
        }

        /// <summary>
        /// Checker name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// create DbConnection and Open
        /// </summary>
        /// <returns></returns>
        public virtual Task<HealthCheckResult> CheckAsync()
        {
            using (var con = this.factory())
            {
                con.Open();
                return Task.FromResult(new HealthCheckResult { Name = this.Name, IsHealthy = true });
            }
        }





    }
}
