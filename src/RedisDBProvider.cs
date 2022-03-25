using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Apteryx.StackExChange.Redis.Extend
{
    public abstract class RedisDBProvider/*:IDisposable*/
    {
        public IDatabase Database { get; private set; }
        private static ConnectionMultiplexer _redisClient;
        private RedisDBProvider() { }

        public RedisDBProvider(IOptionsMonitor<RedisOptions> options) : this(options.CurrentValue.ConnectionString) { }
        public RedisDBProvider(string conn)
        {
            if (_redisClient == null)
                _redisClient = ConnectionMultiplexer.Connect(conn);
            Database = _redisClient.GetDatabase();
        }
    }
}
