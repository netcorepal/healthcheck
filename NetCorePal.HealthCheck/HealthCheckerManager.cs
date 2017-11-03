using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// 健康检查管理器
    /// </summary>
    public class HealthCheckerManager : Collection<IHealthChecker>
    {
        /// <summary>
        /// 健康检查管理器实例，该类仅单例模式
        /// </summary>
        public static HealthCheckerManager Manager { get; private set; } = new HealthCheckerManager();

        private HealthCheckerManager()
        { }


        /// <summary>
        /// 调用所有检查器检查状态并返回
        /// </summary>
        /// <returns>返回所有检查结果</returns>
        public async Task<HealthCheckResult[]> CheckAllAsync()
        {
            var r = await Task.WhenAll(this.Select(p =>
            {
                return DoCheck(p);
            }
            ));
            return r;
        }

        async Task<HealthCheckResult> DoCheck(IHealthChecker checker)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                var result = await checker.CheckAsync().ConfigureAwait(false);
                sw.Stop();
                if (result == null)
                {
                    result = new HealthCheckResult
                    {
                        Name = checker.Name,
                        IsHealthy = true,
                        Elapsed = sw.ElapsedMilliseconds,
                        Message = null,
                        Exception = null
                    };
                }
                else
                {
                    result.Elapsed = sw.ElapsedMilliseconds;
                }
                return result;
            }
            catch (Exception ex)
            {
                sw.Stop();
                return new HealthCheckResult { Name = checker.Name, IsHealthy = false, Exception = ex, Elapsed = sw.ElapsedMilliseconds, Message = ex.Message };
            }
        }
    }
}
