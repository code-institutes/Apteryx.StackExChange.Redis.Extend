using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Apteryx.StackExChange.Redis.Extend
{
    public interface IRedisCollection<T>
        where T : BaseRedisEntity
    {
        T FirstOrDefault(Func<T, bool> predicate);
        T Find(string key);
        Task<T> FindAsync(string key);

        bool Add(T value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None);
        bool Add(T value, DateTime? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None);

        bool Add(string key, T value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None);
        bool Add(string key, T value, DateTime? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None);

        Task<bool> AddAsync(T value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None);
        Task<bool> AddAsync(T value, DateTime? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None);

        Task<bool> AddAsync(string key, T value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None);
        Task<bool> AddAsync(string key, T value, DateTime? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None);

        void AddRange(IEnumerable<T> values, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None);
        void AddRange(IEnumerable<T> values, DateTime? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None);

        Task AddRangeAsync(IEnumerable<T> values, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None);
        Task AddRangeAsync(IEnumerable<T> values, DateTime? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None);

        IEnumerable<T> FindAll();

        bool Remove(T value);
        Task<bool> RemoveAsync(T value);

        bool Remove(Func<T, bool> predicate);
        Task<bool> RemoveAsync(Func<T, bool> predicate);

        long RemoveRange();
        Task RemoveRangeAsync();
    }
}
