using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using NetCorePal.HealthCheck;
namespace NetCorePal.HealthCheck.Test
{
    [TestClass]
    public class HealthCheckerResultHelperTest
    {
        [TestMethod]
        public void HealthCheckerResultHelper_ToHtml_Test()
        {
            var results = new HealthCheckResult[] {

            };


            var html = results.ToHtml();


            Assert.IsNotNull(html);
        }
    }
}
