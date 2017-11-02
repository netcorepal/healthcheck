using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// 健康检查管理器
    /// </summary>
    public class HealthCheckerManager
    {
        /// <summary>
        /// 健康检查管理器实例，该类仅单例模式
        /// </summary>
        public static HealthCheckerManager Manager { get; private set; } = new HealthCheckerManager();

        private HealthCheckerManager()
        { }
        private ConcurrentDictionary<string, IHealthChecker> checkers = new ConcurrentDictionary<string, IHealthChecker>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="checker"></param>
        public void RegisterChecker(IHealthChecker checker)
        {
            if (!this.checkers.TryAdd(checker.Name, checker))
            {
                throw new ArgumentException($"已存在名称为:'{checker.Name}'的checker");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<HealthCheckResult[]> CheckAllAsync()
        {
            var r = await Task.WhenAll(checkers.Values.Select(p =>
            {
                var task = DoCheck(p);
                if (task.Status == TaskStatus.WaitingForActivation)
                {
                    task.Start();
                }
                return task;
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
