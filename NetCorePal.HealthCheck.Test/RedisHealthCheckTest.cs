using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCorePal.HealthCheck.Redis;
namespace NetCorePal.HealthCheck.Test
{
    [TestClass]
    public class RedisHealthCheckTest
    {
        [TestMethod]
        public void CheckAsync_OK_Test()
        {
            var checker = new RedisHealthChecker("Redis", "localhost:6379,defaultDatabase=1");
            var r = checker.CheckAsync().Result;
            Assert.IsNull(r);
        }


        [TestMethod]
        public void CheckAsync_Dead_Test()
        {
            var checker = new RedisHealthChecker("Redis", "localhost:6380,defaultDatabase=1");

            Assert.ThrowsException<AggregateException>(() => checker.CheckAsync().Result);
        }
    }
}
