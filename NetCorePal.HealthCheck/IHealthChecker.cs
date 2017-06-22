using System;
using System.Threading.Tasks;

namespace NetCorePal.HealthCheck
{
    public interface IHealthChecker
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 描述
        /// </summary>
        string Description { get; }
        /// <summary>
        /// 检查方法
        /// </summary>
        Task<IHealthCheckResult> CheckAsync();
    }
}
