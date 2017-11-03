using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.HealthCheck
{
    class AsyncDelegateHealthChecker : IHealthChecker
    {
        public string Name { get; set; }

        public Func<Task<HealthCheckResult>> Func { get; set; }

        public Task<HealthCheckResult> CheckAsync()
        {
            return Func.Invoke();
        }
    }

    class DelegateHealthChecker : HealthChecker
    {
        public Func<HealthCheckResult> Func { get; set; }

        protected override HealthCheckResult Check()
        {
            return Func.Invoke();
        }
    }
}
