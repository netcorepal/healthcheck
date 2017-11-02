using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// 
    /// </summary>
    public class HealthChecker : IHealthChecker
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual async Task<HealthCheckResult> CheckAsync()
        {
            return await Task.Factory.StartNew(Check).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual HealthCheckResult Check()
        {
            return null;
        }
    }
}
