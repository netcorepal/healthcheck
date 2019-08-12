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
    /// checker manager
    /// </summary>
    public class HealthCheckerManager : Collection<IHealthChecker>
    {
        /// <summary>
        /// current manager
        /// </summary>
        public static HealthCheckerManager Manager { get; private set; } = new HealthCheckerManager();

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
