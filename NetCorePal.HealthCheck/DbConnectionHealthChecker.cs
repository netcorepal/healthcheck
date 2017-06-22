using System;
using System.Threading.Tasks;

namespace NetCorePal.HealthCheck
{
    public class DbConnectionHealthChecker : IHealthChecker
    {
        public string Name => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        public Task<IHealthCheckResult> CheckAsync()
        {
            throw new NotImplementedException();
        }
    }
}
