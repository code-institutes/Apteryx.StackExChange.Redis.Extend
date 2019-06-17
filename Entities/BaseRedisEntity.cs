using System;

namespace apteryx.stackexchange.redis.extend.Entities
{
    public abstract class BaseRedisEntity:IRedisEntity
    {
        public string _key { get; private set; } = Guid.NewGuid().ToString();
    }
}
