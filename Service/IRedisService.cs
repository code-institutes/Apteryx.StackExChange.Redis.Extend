using StackExchange.Redis;

namespace Apteryx.StackExChange.Redis.Extend.Service
{
    public interface IRedisService
    {
        IDatabase Database { get; }
        ConnectionMultiplexer RedisClient { get; }
    }
}
