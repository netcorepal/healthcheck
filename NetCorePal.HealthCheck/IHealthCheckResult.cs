using System;

namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// 健康检查
    /// </summary>
    public interface IHealthCheckResult
    {
        bool IsHealthy { get; }

        string Message { get; }

        Exception Exception { get; }
    }
}
