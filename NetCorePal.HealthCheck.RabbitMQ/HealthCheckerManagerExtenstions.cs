using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using NetCorePal.HealthCheck.RabbitMQ;
namespace NetCorePal.HealthCheck
{
    /// <summary>
    /// Extenstions for <see cref="HealthCheckerManager"/> 
    /// </summary>
    public static class HealthCheckerManagerExtenstions
    {
        /// <summary>
        /// add  <see cref="RabbitMQHealthChecker"/> to manager
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="name"></param>
        /// <param name="connectionFactory"></param>
        /// <returns></returns>
        public static HealthCheckerManager AddRabbitMQHealthChecker(this HealthCheckerManager manager, string name, IConnectionFactory connectionFactory)
        {
            manager.Add(new RabbitMQHealthChecker(name, connectionFactory));
            return manager;
        }
    }
}
