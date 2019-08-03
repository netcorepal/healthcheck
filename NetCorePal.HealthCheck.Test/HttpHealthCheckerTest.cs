using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCorePal.HealthCheck.Test
{
    [TestClass]
    public class HttpHealthCheckerTest
    {
        [TestMethod]
        public void CheckAsync_OK_Test()
        {
            var checker = new HttpHealthChecker("mycheker", "https://github.com/");

            Assert.AreEqual("mycheker", checker.Name);
            Assert.IsNull(checker.CheckAsync().Result);
        }


        [TestMethod]
        public void CheckAsync_404_Test()
        {
            var checker = new HttpHealthChecker("mycheker", "https://github.com/thepage/4/0/4");

            Assert.AreEqual("mycheker", checker.Name);
            Assert.ThrowsException<AggregateException>(() => checker.CheckAsync().Result);
        }


        [TestMethod]
        public void CheckAsync_No_Reachable_Test()
        {
            var checker = new HttpHealthChecker("mycheker", "https://192.168.246.246");

            Assert.AreEqual("mycheker", checker.Name);
            Assert.ThrowsException<AggregateException>(() => checker.CheckAsync().Result);
        }


        [TestMethod]
        public void CheckAsync_Head_Method_Test()
        {
            var checker = new HttpHeadHealthChecker("myheadcheker", "https://github.com/thepage/4/0/4");

            Assert.AreEqual("myheadcheker", checker.Name);
            Assert.IsNull(checker.CheckAsync().Result);
        }
    }
}
