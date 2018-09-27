using System;
using StackExchange.Redis;
namespace NetCorePal.HealthCheck.Redis
{
    /// <summary>
    /// Redis Healthy Checker
    /// </summary>
    public class RedisHealthChecker : HealthChecker
    {

        ConnectionMultiplexer connection = null;
        readonly string connectionString;

        /// <summary>
        /// RedisHealthChecker
        /// </summary>
        /// <param name="name">check name</param>
        /// <param name="connectionString">redis connection string</param>
        public RedisHealthChecker(string name, string connectionString)
        {
            this.Name = name;
            this.connectionString = connectionString;
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override HealthCheckResult Check()
        {
            var connection = this.connection ?? ConnectionMultiplexer.Connect(connectionString);
            string key = "healthcheck-" + Guid.NewGuid().ToString("N");
            string value = Guid.NewGuid().ToString("N");

            var database = connection.GetDatabase();
            if (!database.StringSet(key, value, TimeSpan.FromMinutes(1)))
            {
                throw new Exception("Redis随机写入失败");
            }

            if (value != database.StringGet(key))
            {
                throw new Exception("Redis读取数据与写入不一致");
            }
            return base.Check();
        }


    }
}
