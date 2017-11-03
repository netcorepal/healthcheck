using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.HealthCheck.Test
{
    [TestClass]
    public class HealthCheckerManagerTest
    {
        [TestMethod]
        public void Manager_Instance_Test()
        {
            Assert.IsNotNull(HealthCheckerManager.Manager);
            Assert.AreSame(HealthCheckerManager.Manager, HealthCheckerManager.Manager);
        }

        [TestMethod]
        public void CheckAllAsync_Test()
        {
            var manager = HealthCheckerManager.Manager;
            manager.Clear();
            manager.Add(new FakeChecker { Name = "fake checker", Result = new HealthCheckResult { Name = "f1", Elapsed = -1, Exception = null, IsHealthy = true, Message = "m1" } });

            var r = manager.CheckAllAsync().Result;
            Assert.AreEqual(1, r.Length);
            Assert.AreEqual("f1", r[0].Name);
            Assert.IsTrue(r[0].Elapsed > 0);
            Assert.AreEqual("m1", r[0].Message);

        }


        class FakeChecker : IHealthChecker
        {
            public string Name { get; set; }

            public HealthCheckResult Result { get; set; }


            public Task<HealthCheckResult> CheckAsync()
            {
                return Task.Run(() =>
                {
                    System.Threading.Thread.Sleep(1);
                    return this.Result;
                });
            }
        }

    }
}
