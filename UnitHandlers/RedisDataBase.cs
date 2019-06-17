using apteryx.stackexchange.redis.extend.Entities;
using StackExchange.Redis;

namespace Apteryx.StackExChange.Redis.Extend.UnitHandlers
{
    public static class RedisDataBase
    {
        public static IRedisCollection<T> GetCollection<T>(this IDatabase db) where T : BaseRedisEntity
        {
            return new RedisCollection<T>(db);
        }
    }
}
