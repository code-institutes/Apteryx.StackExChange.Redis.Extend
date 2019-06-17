using System;
using System.Collections.Generic;
using System.Text;

namespace apteryx.stackexchange.redis.extend.Entities
{
    public interface IRedisEntity
    {
        string _key { get; }
    }
}
