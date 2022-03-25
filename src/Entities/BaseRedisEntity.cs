using System;

namespace Apteryx.StackExChange.Redis.Extend
{
    public abstract class BaseRedisEntity
    {
        public string _key { get; private set; } = Guid.NewGuid().ToString();
    }
}
