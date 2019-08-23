using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Collections.ObjectModel;
#if NET45
#else
using Microsoft.Extensions.Logging;
#endif
namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// checker manager
    /// </summary>
    public class HealthCheckerManager : Collection<IHealthChecker>
    {
        /// <summary>
        /// current manager
        /// </summary>
        public static HealthCheckerManager Manager { get; private set; } = new HealthCheckerManager();
#if NET45
#else
        internal ILogger Logger;
        int WarnningTimes = 1000; //log warnning if all checker elapsed up on x (ms);
#endif


        private HealthCheckerManager()
        { }


        /// <summary>
        /// check all checker
        /// </summary>
        /// <returns></returns>
        public async Task<HealthCheckResult[]> CheckAllAsync()
        {
            var r = await Task.WhenAll(this.Select(async p =>
            {
                return await DoCheck(p).ConfigureAwait(false);
            }
            )).ConfigureAwait(false);
#if NET45
#else
            if (this.Logger != null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var result in r)
                {
                    sb.AppendFormat("Name:{0} ,Message:{1}, Elapsed:{2}, IsHealthy:{3} ,ExceptionMessage:{4}, StackTrace:{5}", result.Name, result.Message, result.Elapsed, result.IsHealthy, result.Exception?.Message, result.Exception?.StackTrace);
                    sb.AppendLine();
                }
                if (r.Any(p => !p.IsHealthy))
                {
                    Logger.LogError(sb.ToString());
                }
                else if (r.Any(p => p.Elapsed > WarnningTimes))
                {
                    Logger.LogWarning(sb.ToString());
                }
                else
                {
                    Logger.LogInformation(sb.ToString());
                }
            }
#endif
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
