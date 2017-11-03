using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetCorePal.HealthCheck.Test
{
    [TestClass]
    public class DbConnectionHealthCheckerTest
    {
        internal const string connectionString = "server=localhost;user id=root;password=123456;";
        [TestMethod]
        public void DbConnectionHealth_CheckAsync_Test()
        {
            var checker = new DbConnectionHealthChecker("n1", new MySql.Data.MySqlClient.MySqlClientFactory(), connectionString);

            var r = checker.CheckAsync().Result;
            Assert.AreEqual("n1", r.Name);
            Assert.IsTrue(r.IsHealthy);
            Assert.IsNull(r.Exception);
            Assert.IsNull(r.Message);
            Assert.AreEqual(0, r.Elapsed);
        }
#if NET45
        [TestMethod]
        public void DbConnectionHealth_CheckAsync_Test_Net45()
        {
            var checker = new DbConnectionHealthChecker("n1", "MySql.Data.MySqlClient", connectionString);
            var r = checker.CheckAsync().Result;
            Assert.AreEqual("n1", r.Name);
            Assert.IsTrue(r.IsHealthy);
            Assert.IsNull(r.Exception);
            Assert.IsNull(r.Message);
            Assert.AreEqual(0, r.Elapsed);
        }
#endif
    }
}
