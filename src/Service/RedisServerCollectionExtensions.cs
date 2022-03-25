using System;
using Microsoft.Extensions.DependencyInjection;

namespace Apteryx.StackExChange.Redis.Extend
{
    public static class RedisServerCollectionExtensions
    {
        public static IServiceCollection AddRedis<T>(this IServiceCollection serviceCollection,Action<RedisOptions> optionsAction) where T : RedisDBProvider
        {
            serviceCollection.AddSingleton<T>();
            if (optionsAction != null)
                serviceCollection.Configure(optionsAction);
            return serviceCollection;
        }
    }
}
