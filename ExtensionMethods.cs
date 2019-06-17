using Apteryx.StackExChange.Redis.Extend.UnitHandlers;
using StackExchange.Redis;

namespace Apteryx.StackExChange.Redis.Extend
{
    public static class ExtensionMethods
    {
        public static StringListHandler<T> AsClientType<T>(this IDatabase db) where T: class
        {
            return new StringListHandler<T>(db);
        }
    }
}
