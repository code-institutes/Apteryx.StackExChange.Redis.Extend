using System;
using System.Collections.Generic;
using StackExchange.Redis;

namespace Apteryx.StackExChange.Redis.Extend.UnitHandlers
{
    public interface IUnit<T>
    where T:class 
    {
        long Append(T value, CommandFlags flags = CommandFlags.None);
        long Append(T value, TimeSpan? expiry, CommandFlags flags = CommandFlags.None);
        long Append(T value, DateTime? expiry, CommandFlags flags = CommandFlags.None);
        IEnumerable<T> GetAll();
        long DeleletAll();
    }
}
