using StackExchange.Redis;

namespace Apteryx.StackExChange.Redis.Extend
{
    public static class RedisDataHelper
    {
        public static IRedisCollection<T> GetCollection<T>(this IDatabase db) where T : BaseRedisEntity
        {
            return new RedisCollection<T>(db);
        }
    }
}
