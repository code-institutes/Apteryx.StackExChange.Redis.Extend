using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using apteryx.stackexchange.redis.extend.Entities;
using StackExchange.Redis;

namespace Apteryx.StackExChange.Redis.Extend.UnitHandlers
{
    public interface IUnit<T>: IDisposable
    where T: BaseRedisEntity
    {
        long Append(T value, CommandFlags flags = CommandFlags.None);
        Task<long> AppendAsync(T value, CommandFlags flags = CommandFlags.None);

        long Append(T value, TimeSpan? expiry, CommandFlags flags = CommandFlags.None);
        Task<long> AppendAsync(T value, TimeSpan? expiry, CommandFlags flags = CommandFlags.None);

        long Append(T value, DateTime? expiry, CommandFlags flags = CommandFlags.None);
        Task<long> AppendAsync(T value, DateTime? expiry, CommandFlags flags = CommandFlags.None);

        IEnumerable<T> GetAll();

        T FirstOrDefault(Func<T, bool> predicate);

        long DeleletAll();
        Task DeleletAllAsync();

        bool Delete(T value);
        Task<bool> DeleteAsync(T value);

        bool Delete(Func<T, bool> predicate);
        Task<bool> DeleteAsync(Func<T, bool> predicate);
    }
}
