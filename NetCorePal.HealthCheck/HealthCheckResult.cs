using System;

namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// Check Result
    /// </summary>
    public class HealthCheckResult
    {
        /// <summary>
        /// checker name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// is healthy or not
        /// </summary>
        public bool IsHealthy { get; set; }
        /// <summary>
        /// message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// exception
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// check elapsed by milliseconds 
        /// </summary>
        public long Elapsed { get; set; }
    }
}
