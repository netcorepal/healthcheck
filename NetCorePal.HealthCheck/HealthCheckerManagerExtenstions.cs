using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// extenstions for <seealso cref="HealthCheckerManager"/>
    /// </summary>
    public static class HealthCheckerManagerExtenstions
    {
#if NET45
        /// <summary>
        /// using web.config  or app.config connectionStrings for checker
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="skipLocalSqlServer">skip connectionStrings named "LocalSqlServer"  ,default by true</param>
        public static void AddAllDbConnectionHealthCheckers(this HealthCheckerManager manager, bool skipLocalSqlServer = true)
        {
            for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
            {
                var con = ConfigurationManager.ConnectionStrings[i];
                if (con.Name == "LocalSqlServer" && skipLocalSqlServer)
                {
                    continue;
                }
                manager.Add(new DbConnectionHealthChecker(con.Name, con.ProviderName, con.ConnectionString));
            }
        }
#endif
        /// <summary>
        /// add  DbConnectionHealthChecker to manager
        /// </summary>
        /// <param name="manager">instance of HealthCheckerManager</param>
        /// <param name="name">checker name</param>
        /// <param name="factory">factory</param>
        /// <param name="connectionString">connectionString</param>
        public static void AddDbConnectionHealthChecker(this HealthCheckerManager manager, string name, DbProviderFactory factory, string connectionString)
        {
            manager.Add(new DbConnectionHealthChecker(name, factory, connectionString));
        }

        /// <summary>
        /// add  DbConnectionHealthChecker to manager
        /// </summary>
        /// <param name="manager">instance of HealthCheckerManager</param>
        /// <param name="name">checker name</param>
        /// <param name="factory">IDbConnection factory</param>    
        public static void AddDbConnectionHealthChecker(this HealthCheckerManager manager, string name, Func<IDbConnection> factory)
        {
            manager.Add(new DbConnectionHealthChecker(name, factory));
        }

        /// <summary>
        /// add checker
        /// </summary>
        /// <param name="manager">instance of HealthCheckerManager</param>
        /// <param name="name">checker name</param>
        /// <param name="func">check func</param>
        public static void Add(this HealthCheckerManager manager, string name, Func<Task<HealthCheckResult>> func)
        {
            manager.Add(new AsyncDelegateHealthChecker { Name = name, Func = func });
        }

        /// <summary>
        /// add checker
        /// </summary>
        /// <param name="manager">instance of HealthCheckerManager</param>
        /// <param name="name">checker name</param>
        /// <param name="func">check func</param>
        public static void Add(this HealthCheckerManager manager, string name, Func<HealthCheckResult> func)
        {
            manager.Add(new DelegateHealthChecker { Name = name, Func = func });
        }
        /// <summary>
        /// add <see cref="HttpHealthChecker"/>  
        /// </summary>
        /// <param name="manager">instance of HealthCheckerManager</param>
        /// <param name="name">checker name</param>
        /// <param name="url">http url to check</param>
        public static void AddHttpHealthChecker(this HealthCheckerManager manager, string name, string url)
        {
            manager.Add(new HttpHealthChecker(name, url));
        }

        /// <summary>
        /// add <see cref="HttpHeadHealthChecker"/>  
        /// </summary>
        /// <param name="manager">instance of HealthCheckerManager</param>
        /// <param name="name">checker name</param>
        /// <param name="url">http url to check</param>
        public static void AddHttpHeadHealthChecker(this HealthCheckerManager manager, string name, string url)
        {
            manager.Add(new HttpHeadHealthChecker(name, url));
        }
    }
}
