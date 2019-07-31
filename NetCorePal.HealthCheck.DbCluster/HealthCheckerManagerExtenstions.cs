using NetCorePal.HealthCheck.DbCluster;
using System.Configuration;

namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// Extenstions for <see cref="HealthCheckerManager"/> 
    /// </summary>
    public static class HealthCheckerManagerExtenstions
    {
        /// <summary>
        /// add  <see cref="DbClusterHealthCheck"/> to manager
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public static HealthCheckerManager AddDbClusterHealthChecker(this HealthCheckerManager manager, string dbclusterSection = "databaseClusters")
        {
            var cluster = ConfigurationManager.GetSection(dbclusterSection) as SchoolPal.Toolkit.Configuration.DatabaseClusters.DatabaseClusterSection;

            if (cluster != null)
            {
                var clusters = cluster.GetClusters();
                foreach (var conn in clusters)
                {
                    manager.Add(new DbClusterHealthCheck(conn.Name, conn.Master));

                    var slaves = conn.GetSlaveConnectionStrings();
                    for (int i = 0; i < slaves.Length; i++)
                    {
                        manager.Add(new DbClusterHealthCheck($"{conn.Name}:Slave{i}", slaves[i]));
                    }

                }
            }

            return manager;
        }
    }
}
