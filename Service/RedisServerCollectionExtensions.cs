using System;
using Apteryx.StackExChange.Redis.Extend.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Apteryx.StackExChange.Redis.Extend.Service
{
    public static class RedisServerCollectionExtensions
    {
        public static IServiceCollection AddRedis<T>(this IServiceCollection serviceCollection,
            Action<RedisOptions> optionsAction) where T : RedisDBProvider 
        {
            serviceCollection.AddScoped<IRedisService, T>().BuildServiceProvider();
            if (optionsAction != null)
                serviceCollection.ConfigureMongoDB(optionsAction);
            return serviceCollection;
        }
        public static void ConfigureMongoDB(this IServiceCollection services, Action<RedisOptions> optionsAction)
        {
            services.Configure(optionsAction);
        }
    }
}
