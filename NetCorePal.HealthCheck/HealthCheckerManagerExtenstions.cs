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
        /// 添加数据库连接字符串健康检查器
        /// </summary>
        /// <param name="manager">健康检查管理器实例</param>
        /// <param name="name">检查器名称</param>
        /// <param name="factory">数据库提供程序工厂实例</param>
        /// <param name="connectionString">连接字符串</param>
        public static void AddDbConnectionHealthChecker(this HealthCheckerManager manager, string name, DbProviderFactory factory, string connectionString)
        {
            manager.Add(new DbConnectionHealthChecker(name, factory, connectionString));
        }

        /// <summary>
        /// 添加健康检查器
        /// </summary>
        /// <param name="manager">健康检查管理器实例</param>
        /// <param name="name">检查器名称</param>
        /// <param name="func">异步检查方法</param>
        public static void Add(this HealthCheckerManager manager, string name, Func<Task<HealthCheckResult>> func)
        {
            manager.Add(new AsyncDelegateHealthChecker { Name = name, Func = func });
        }

        /// <summary>
        /// 添加健康检查器
        /// </summary>
        /// <param name="manager">健康检查管理器实例</param>
        /// <param name="name">检查器名称</param>
        /// <param name="func">检查方法</param>
        public static void Add(this HealthCheckerManager manager, string name, Func<HealthCheckResult> func)
        {
            manager.Add(new DelegateHealthChecker { Name = name, Func = func });
        }
    }
}
