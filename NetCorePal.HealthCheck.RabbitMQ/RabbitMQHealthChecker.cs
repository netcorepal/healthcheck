using RabbitMQ.Client;
using System;

namespace NetCorePal.HealthCheck.RabbitMQ
{
    /// <summary>
    /// 基于 RabbitMQ.Client 的 RabbitMQ  HealthChecker
    /// </summary>
    public class RabbitMQHealthChecker : HealthChecker
    {
        readonly IConnectionFactory connectionFactory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">Checker 自定义名称</param>
        /// <param name="connectionFactory">rebbitmq 连接工厂对象</param>
        public RabbitMQHealthChecker(string name, IConnectionFactory connectionFactory)
        {
            this.Name = name;
            this.connectionFactory = connectionFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override HealthCheckResult Check()
        {
            using (var connection = this.connectionFactory.CreateConnection())
            {

            }
            return base.Check();
        }
    }
}
