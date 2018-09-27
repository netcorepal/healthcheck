using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCorePal.HealthCheck.Redis;
namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// Extenstions for <see cref="HealthCheckerManager"/>
    /// </summary>
    public static class HealthCheckerManagerExtenstions
    {
        /// <summary>
        /// add <see cref="RedisHealthChecker"/> to <see cref="HealthCheckerManager"/>
        /// </summary>
        /// <param name="manager">instance of <see cref="HealthCheckerManager"/></param>
        /// <param name="name">checker name</param>
        /// <param name="connectionString">redis connection string</param>
        /// <returns><see cref="HealthCheckerManager"/></returns>
        public static HealthCheckerManager AddRedisHealthChecker(this HealthCheckerManager manager, string name, string connectionString)
        {
            manager.Add(new RedisHealthChecker(name, connectionString));
            return manager;
        }
    }
}
