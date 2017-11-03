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
        /// 检查器名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 发起异步检查，返回的HealthCheckResult表示检查结果，如果抛出异常，则检查管理器将构造一个表示异常的HealthCheckResult，如果返回null，则检查管理器构造一个表示正常的HealthCheckResult
        /// </summary>
        Task<HealthCheckResult> CheckAsync();
    }
}
