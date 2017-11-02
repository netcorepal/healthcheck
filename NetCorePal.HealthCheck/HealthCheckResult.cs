using System;

namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// 健康检查
    /// </summary>
    public class HealthCheckResult
    {
        /// <summary>
        /// 检查器名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否健康
        /// </summary>
        public bool IsHealthy { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 检查耗时,单位毫秒
        /// </summary>
        public long Elapsed { get; set; }
    }
}
