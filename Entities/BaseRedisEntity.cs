
using System;
using System.Collections.Generic;
using System.Text;

namespace apteryx.stackexchange.redis.extend.Entities
{
    public abstract class BaseRedisEntity:IRedisEntity
    {
        public string _key { get; protected set; }
    }
}
