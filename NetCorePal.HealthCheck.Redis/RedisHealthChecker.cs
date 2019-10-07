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
        IDatabase database = null;
        readonly string connectionString;
        bool inited; //表示connection、database是否成功初始化
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
            if (!inited)
            {
                lock (this)
                {
                    //使用空判断避免并发重复进入而重复执行创建连接的操作
                    this.connection = this.connection ?? ConnectionMultiplexer.Connect(connectionString);
                    this.database = this.database ?? connection.GetDatabase();
                    inited = true;
                }
            }
            string key = "healthcheck-" + Guid.NewGuid().ToString("N");
            string value = Guid.NewGuid().ToString("N");
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
