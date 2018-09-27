using System;
using System.Threading.Tasks;

namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// health checker interface  
    /// </summary>
    public interface IHealthChecker
    {
        /// <summary>
        /// checker name
        /// </summary>
        string Name { get; }
        /// <summary>
        /// do check async，return null if ok,throw exception if check fails. 
        /// </summary>
        Task<HealthCheckResult> CheckAsync();
    }
}
