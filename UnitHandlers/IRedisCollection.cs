using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using apteryx.stackexchange.redis.extend.Entities;
using StackExchange.Redis;

namespace Apteryx.StackExChange.Redis.Extend.UnitHandlers
{
    public interface IRedisCollection<T>
        where T : BaseRedisEntity
    {
        T FirstOrDefault(Func<T, bool> predicate);
        T Find(string key);
        Task<T> FindAsync(string key);

        long Add(T value, CommandFlags flags = CommandFlags.None);
        bool Add(T value, TimeSpan? expiry, CommandFlags flags = CommandFlags.None);
        bool Add(T value, DateTime? expiry, CommandFlags flags = CommandFlags.None);

        Task<long> AddAsync(T value, CommandFlags flags = CommandFlags.None);
        Task<bool> AddAsync(T value, TimeSpan? expiry, CommandFlags flags = CommandFlags.None);
        Task<bool> AddAsync(T value, DateTime? expiry, CommandFlags flags = CommandFlags.None);

        void AddRange(IEnumerable<T> values, CommandFlags flags = CommandFlags.None);
        void AddRange(IEnumerable<T> values, TimeSpan? expiry, CommandFlags flags = CommandFlags.None);
        void AddRange(IEnumerable<T> values, DateTime? expiry, CommandFlags flags = CommandFlags.None);

        Task AddRangeAsync(IEnumerable<T> values, CommandFlags flags = CommandFlags.None);
        Task AddRangeAsync(IEnumerable<T> values, TimeSpan? expiry, CommandFlags flags = CommandFlags.None);
        Task AddRangeAsync(IEnumerable<T> values, DateTime? expiry, CommandFlags flags = CommandFlags.None);

        IEnumerable<T> FindAll();

        bool Remove(T value);
        Task<bool> RemoveAsync(T value);

        bool Remove(Func<T, bool> predicate);
        Task<bool> RemoveAsync(Func<T, bool> predicate);

        long RemoveRange();
        Task RemoveRangeAsync();
    }
}
