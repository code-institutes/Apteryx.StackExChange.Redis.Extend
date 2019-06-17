using System;
using Apteryx.StackExChange.Redis.Extend.Entities;
using Apteryx.StackExChange.Redis.Extend.Service;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Apteryx.StackExChange.Redis.Extend
{
    public abstract class RedisDBProvider:IRedisService,IDisposable
    {
        public IDatabase Database { get; private set; }
        public ConnectionMultiplexer RedisClient { get; private set; }
        private RedisDBProvider() { }

        public RedisDBProvider(IOptionsMonitor<RedisOptions> options)
        {
            if (RedisClient == null)
                RedisClient = ConnectionMultiplexer.Connect(options.CurrentValue.ConnectionString);
            Database = RedisClient.GetDatabase();
        }
        public RedisDBProvider(string conn)
        {
            if (RedisClient == null)
                RedisClient = ConnectionMultiplexer.Connect(conn);
            Database = RedisClient.GetDatabase();
        }

        public void Close()
        {
            RedisClient.Close();
        }

        public void Dispose()
        {
            RedisClient.Close();
        }
    }
}
