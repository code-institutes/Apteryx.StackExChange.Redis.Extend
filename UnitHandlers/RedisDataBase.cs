using StackExchange.Redis;

namespace Apteryx.StackExChange.Redis.Extend.UnitHandlers
{
    public static class RedisDataBase
    {
        public static IRedisCollection<T> GetCollection<T>(this IDatabase db) where T : class
        {
            return new RedisCollection<T>(db);
        }
    }
}
