using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// 健康检查管理器扩展
    /// </summary>
    public static class HealthCheckerManagerExtenstions
    {
#if NET45
        /// <summary>
        /// 注册web.config或app.config 中connectionStrings的连接字符串到checker
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="skipLocalSqlServer">是否跳过默认连接字符串“LocalSqlServer”,默认true</param>
        public static void RegisterAllDbConnectionHealthCheckers(this HealthCheckerManager manager, bool skipLocalSqlServer = true)
        {
            for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
            {
                var con = ConfigurationManager.ConnectionStrings[i];
                if (con.Name == "LocalSqlServer" && skipLocalSqlServer)
                {
                    continue;
                }
                manager.Add(new DbConnectionHealthChecker(con.Name, con.ConnectionString, con.ProviderName));
            }
        }
#endif
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="name"></param>
        /// <param name="factory"></param>
        /// <param name="connectionString"></param>
        public static void RegisterDbConnectionHealthChecker(this HealthCheckerManager manager, string name, DbProviderFactory factory, string connectionString)
        {
            manager.Add(new DbConnectionHealthChecker(name, factory, connectionString));
        }
    }
}
