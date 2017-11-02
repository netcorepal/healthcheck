using System;
using System.Threading.Tasks;

namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// 健康检查接口
    /// </summary>
    public interface IHealthChecker
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 检查
        /// </summary>
        Task<HealthCheckResult> CheckAsync();
    }
}
