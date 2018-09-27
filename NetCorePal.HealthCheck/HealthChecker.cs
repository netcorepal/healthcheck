using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// HealthChecker Base Class
    /// </summary>
    public class HealthChecker : IHealthChecker
    {
        /// <summary>
        /// Checker Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Do Check Async
        /// </summary>
        /// <returns></returns>
        public virtual async Task<HealthCheckResult> CheckAsync()
        {
            return await Task.Factory.StartNew(Check).ConfigureAwait(false);
        }

        /// <summary>
        /// Do nothing
        /// </summary>
        /// <returns>always null</returns>
        protected virtual HealthCheckResult Check()
        {
            return null;
        }
    }
}
