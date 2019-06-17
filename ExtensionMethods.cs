using apteryx.stackexchange.redis.extend.Entities;
using Apteryx.StackExChange.Redis.Extend.UnitHandlers;
using StackExchange.Redis;

namespace Apteryx.StackExChange.Redis.Extend
{
    public static class ExtensionMethods
    {
        public static IUnit<T> AsClientType<T>(this IDatabase db) where T: BaseRedisEntity
        {
            return new StringListHandler<T>(db);
        }
    }
}
