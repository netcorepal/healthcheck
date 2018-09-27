using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCorePal.HealthCheck.RabbitMQ;
using RabbitMQ.Client;

namespace NetCorePal.HealthCheck.Test
{
    [TestClass]
    public class RabbitMQHealthCheckerTest
    {
        [TestMethod]
        public void CheckAsync_OK_Test()
        {
            var checker = new RabbitMQHealthChecker("MQ", new ConnectionFactory());            //use localhost:5672: guest guest
            var r = checker.CheckAsync().Result;
            Assert.IsNull(r);
        }


        [TestMethod]
        public void CheckAsync_Dead_Test()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory
            {
                HostName = "",
                Port = 123
            };
            var checker = new RabbitMQHealthChecker("MQ", connectionFactory);

            Assert.ThrowsException<AggregateException>(() => checker.CheckAsync().Result);
        }
    }
}
