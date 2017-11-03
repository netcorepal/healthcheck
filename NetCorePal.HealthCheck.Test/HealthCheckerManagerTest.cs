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
        public void HealthCheckerManager_Instance_Test()
        {
            Assert.IsNotNull(HealthCheckerManager.Manager);
            Assert.AreSame(HealthCheckerManager.Manager, HealthCheckerManager.Manager);
        }

#if NET45
        [TestMethod]
        public void HealthCheckerManager_RegisterAllDbConnectionHealthCheckers_Test()
        {
            var manager = HealthCheckerManager.Manager;
            manager.Clear();
            manager.AddAllDbConnectionHealthCheckers(false);
            Assert.AreEqual(2, manager.Count);

            Assert.AreEqual("LocalSqlServer", manager[0].Name);
            Assert.AreEqual("testdb", manager[1].Name);

            manager.Clear();
            manager.AddAllDbConnectionHealthCheckers();
            Assert.AreEqual(1, manager.Count);
            Assert.AreEqual("testdb", manager[0].Name);

            var r = manager.CheckAllAsync().Result;

            Assert.AreEqual(1, r.Length);
            Assert.AreEqual("testdb", r[0].Name);
            Assert.IsTrue(r[0].IsHealthy);
            Assert.IsNull(r[0].Message);
            Assert.IsNull(r[0].Exception);
            manager.Clear();
        }
#endif

        [TestMethod]
        public void HealthCheckerManager_RegisterDbConnectionHealthChecker_Test()
        {
            var manager = HealthCheckerManager.Manager;
            manager.Clear();

            manager.AddDbConnectionHealthChecker("DB", new MySql.Data.MySqlClient.MySqlClientFactory(), DbConnectionHealthCheckerTest.connectionString);
            Assert.AreEqual(1, manager.Count);
            Assert.AreEqual("DB", manager[0].Name);
            var r = manager.CheckAllAsync().Result;

            Assert.AreEqual(1, r.Length);
            Assert.AreEqual("DB", r[0].Name);
            Assert.IsTrue(r[0].IsHealthy);
            Assert.IsNull(r[0].Message);
            Assert.IsNull(r[0].Exception);
        }
        [TestMethod]
        public void HealthCheckerManager_CheckAllAsync_Test()
        {
            var manager = HealthCheckerManager.Manager;
            manager.Clear();
            manager.Add(new FakeChecker { Name = "fake checker", Result = new HealthCheckResult { Name = "f1", Elapsed = -1, Exception = null, IsHealthy = true, Message = "m1" } });

            var r = manager.CheckAllAsync().Result;
            Assert.AreEqual(1, r.Length);
            Assert.AreEqual("f1", r[0].Name);
            Assert.IsTrue(r[0].Elapsed > 0);
            Assert.IsTrue(r[0].IsHealthy);
            Assert.AreEqual("m1", r[0].Message);
            Assert.IsNull(r[0].Exception);


            manager.Clear();

            manager.Add(new FakeExceptionChecker { Name = "ex" });


            r = manager.CheckAllAsync().Result;
            Assert.AreEqual(1, r.Length);
            Assert.AreEqual("ex", r[0].Name);
            Assert.IsTrue(r[0].Elapsed > 0);
            Assert.IsNotNull(r[0].Exception);
            Assert.IsInstanceOfType(r[0].Exception, typeof(Exception));
            Assert.AreEqual("fake exception", r[0].Exception.Message);
            Assert.AreEqual("fake exception", r[0].Message);
            Assert.IsFalse(r[0].IsHealthy);
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


        class FakeExceptionChecker : IHealthChecker
        {
            public string Name { get; set; }

            public Task<HealthCheckResult> CheckAsync()
            {
                return Task.Run<HealthCheckResult>(new Func<HealthCheckResult>(DoCheck));
            }

            HealthCheckResult DoCheck()
            {
                System.Threading.Thread.Sleep(1);
                throw new Exception("fake exception");
            }


        }

    }
}
