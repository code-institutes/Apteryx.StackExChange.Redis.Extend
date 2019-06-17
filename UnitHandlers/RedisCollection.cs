using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using StackExchange.Redis;

namespace Apteryx.StackExChange.Redis.Extend.UnitHandlers
{
    public class RedisCollection<T> : IRedisCollection<T>
        where T : class
    {
        private bool m_disposed;
        private readonly IDatabase db;
        private StringBuilder _keyPrefix = new StringBuilder();
        public string KeyPrefix => _keyPrefix.ToString();
        public string Key => string.Format("{0}{1}", KeyPrefix, Guid.NewGuid());
        public Func<string, string> DKey => (key) => { return string.Format("{0}{1}", KeyPrefix, key); };

        public RedisCollection(IDatabase database)
        {
            this.db = database;
            BuildKeyPrefix();
        }

        public T FirstOrDefault(Func<T, bool> predicate)
        {
            foreach (var ep in db.Multiplexer.GetEndPoints())
            {
                var server = db.Multiplexer.GetServer(ep);
                var keys = server.Keys(pattern: KeyPrefix + "*", database: db.Database);
                foreach (var k in keys)
                {
                    var obj = db.StringGet(k).ToString().FromJson<T>();
                    if (predicate.Invoke(obj))
                        return obj;
                }
            }

            return default(T);
        }

        public T Find(string key)
        {
            key = DKey(key);
            return db.StringGet(key).ToString().FromJson<T>();
        }

        public async Task<T> FindAsync(string key)
        {
            key = DKey(key);
            return (await db.StringGetAsync(key)).ToString().FromJson<T>();
        }

        public IEnumerable<T> FindAll()
        {
            foreach (var ep in db.Multiplexer.GetEndPoints())
            {
                var server = db.Multiplexer.GetServer(ep);
                var keys = server.Keys(pattern: KeyPrefix + "*", database: db.Database);
                foreach (var k in keys)
                {
                    yield return db.StringGet(k).ToString().FromJson<T>();
                }
            }
        }

        public long Add(T value, CommandFlags flags = CommandFlags.None)
        {
            return db.StringAppend(this.Key, value.ToJson(), flags);
        }

        public bool Add(T value, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            var key = this.Key;
            bool result = false;
            try
            {
                db.StringAppend(key, value.ToJson(), flags);
                result = db.KeyExpire(key, expiry);
            }
            finally
            {
                if (!result)
                {
                    db.KeyDelete(key, flags);
                    Console.WriteLine($"过期时间设置失败！Key：{key}");
                }
            }

            return result;
        }

        public bool Add(T value, DateTime? expiry, CommandFlags flags = CommandFlags.None)
        {
            var key = this.Key;
            bool result = false;
            try
            {
                db.StringAppend(key, value.ToJson(), flags);
                result = db.KeyExpire(key, expiry);
            }
            finally
            {
                if (!result)
                {
                    db.KeyDelete(key, flags);
                    Console.WriteLine($"过期时间设置失败！Key：{key}");
                }
            }

            return result;
        }

        public long Add(string key, T value, CommandFlags flags = CommandFlags.None)
        {
            key = DKey(key);
            db.KeyDelete(key);
            return db.StringAppend(key, value.ToJson(), flags);
        }

        public bool Add(string key, T value, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            key = DKey(key);
            db.KeyDelete(key);
            bool result = false;
            try
            {
                db.StringAppend(key, value.ToJson(), flags);
                result = db.KeyExpire(key, expiry);
            }
            finally
            {
                if (!result)
                {
                    db.KeyDelete(key, flags);
                    Console.WriteLine($"过期时间设置失败！Key：{key}");
                }
            }

            return result;
        }
        public bool Add(string key, T value, DateTime? expiry, CommandFlags flags = CommandFlags.None)
        {
            key = DKey(key);
            db.KeyDelete(key);
            bool result = false;
            try
            {
                db.StringAppend(key, value.ToJson(), flags);
                result = db.KeyExpire(key, expiry);
            }
            finally
            {
                if (!result)
                {
                    db.KeyDelete(key, flags);
                    Console.WriteLine($"过期时间设置失败！Key：{key}");
                }
            }

            return result;
        }

        public Task<long> AddAsync(T value, CommandFlags flags = CommandFlags.None)
        {
            return db.StringAppendAsync(this.Key, value.ToJson(), flags);
        }

        public async Task<bool> AddAsync(T value, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            var key = this.Key;
            bool result = false;
            try
            {
                await db.StringAppendAsync(key, value.ToJson(), flags);
                result = await db.KeyExpireAsync(key, expiry);
            }
            finally
            {
                if (!result)
                {
                    await db.KeyDeleteAsync(key, flags);
                    Console.WriteLine($"过期时间设置失败！Key：{key}");
                }
            }

            return result;
        }

        public async Task<bool> AddAsync(T value, DateTime? expiry, CommandFlags flags = CommandFlags.None)
        {
            var key = this.Key;
            bool result = false;
            try
            {
                await db.StringAppendAsync(key, value.ToJson(), flags);
                result = await db.KeyExpireAsync(key, expiry);
            }
            finally
            {
                if (!result)
                {
                    await db.KeyDeleteAsync(key, flags);
                    Console.WriteLine($"过期时间设置失败！Key：{key}");
                }
            }

            return result;
        }

        public Task<long> AddAsync(string key,T value, CommandFlags flags = CommandFlags.None)
        {
            key = DKey(key);
            db.KeyDeleteAsync(key);
            return db.StringAppendAsync(key, value.ToJson(), flags);
        }

        public async Task<bool> AddAsync(string key,T value, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            key = DKey(key);
            await db.KeyDeleteAsync(key);
            bool result = false;
            try
            {
                await db.StringAppendAsync(key, value.ToJson(), flags);
                result = await db.KeyExpireAsync(key, expiry);
            }
            finally
            {
                if (!result)
                {
                    await db.KeyDeleteAsync(key, flags);
                    Console.WriteLine($"过期时间设置失败！Key：{key}");
                }
            }

            return result;
        }

        public async Task<bool> AddAsync(string key,T value, DateTime? expiry, CommandFlags flags = CommandFlags.None)
        {
            key = this.DKey(key);
            await db.KeyDeleteAsync(key);
            bool result = false;
            try
            {
                await db.StringAppendAsync(key, value.ToJson(), flags);
                result =await db.KeyExpireAsync(key, expiry);
            }
            finally
            {
                if (!result)
                {
                    await db.KeyDeleteAsync(key, flags);
                    Console.WriteLine($"过期时间设置失败！Key：{key}");
                }
            }

            return result;
        }

        public void AddRange(IEnumerable<T> values, CommandFlags flags = CommandFlags.None)
        {
            foreach (var value in values)
                Add(value);
        }

        public void AddRange(IEnumerable<T> values, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            foreach (var value in values)
                Add(value, expiry);
        }

        public void AddRange(IEnumerable<T> values, DateTime? expiry, CommandFlags flags = CommandFlags.None)
        {
            foreach (var value in values)
                Add(value, expiry);
        }

        public async Task AddRangeAsync(IEnumerable<T> values, CommandFlags flags = CommandFlags.None)
        {
            foreach (var value in values)
                await AddAsync(value);
        }

        public async Task AddRangeAsync(IEnumerable<T> values, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            foreach (var value in values)
                await AddAsync(value, expiry);
        }

        public async Task AddRangeAsync(IEnumerable<T> values, DateTime? expiry, CommandFlags flags = CommandFlags.None)
        {
            foreach (var value in values)
                await AddAsync(value, expiry);
        }

        public bool Remove(string key)
        {
            return db.KeyDelete(key);
        }

        public Task<bool> RemoveAsync(string key)
        {
            return db.KeyDeleteAsync(key);
        }

        public bool Remove(Func<T, bool> predicate)
        {
            foreach (var ep in db.Multiplexer.GetEndPoints())
            {
                var server = db.Multiplexer.GetServer(ep);
                var keys = server.Keys(pattern: KeyPrefix + "*", database: db.Database);
                foreach (var k in keys)
                {
                    var obj = db.StringGet(k).ToString().FromJson<T>();
                    if (predicate.Invoke(obj))
                        return db.KeyDelete(k);
                }
            }

            return false;
        }

        public Task<bool> RemoveAsync(Func<T, bool> predicate)
        {
            foreach (var ep in db.Multiplexer.GetEndPoints())
            {
                var server = db.Multiplexer.GetServer(ep);
                var keys = server.Keys(pattern: KeyPrefix + "*", database: db.Database);
                foreach (var k in keys)
                {
                    var obj = db.StringGet(k).ToString().FromJson<T>();
                    if (predicate.Invoke(obj))
                        return db.KeyDeleteAsync(k);
                }
            }

            return Task.Run(() => { return false;});
        }

        public long RemoveRange()
        {
            long count = 0;
            foreach (var ep in db.Multiplexer.GetEndPoints())
            {
                var server = db.Multiplexer.GetServer(ep);
                var keys = server.Keys(pattern: KeyPrefix + "*", database: db.Database);
                foreach (var k in keys)
                {
                    if (db.KeyDelete(k))
                        ++count;
                }
            }

            return count;
        }

        public Task RemoveRangeAsync()
        {
            foreach (var ep in db.Multiplexer.GetEndPoints())
            {
                var server = db.Multiplexer.GetServer(ep);
                var keys = server.Keys(pattern: KeyPrefix + "*", database: db.Database);
                foreach (var k in keys)
                {
                    db.KeyDeleteAsync(k);
                }
            }

            return default(Task);
        }

        private void BuildKeyPrefix()
        {
            _keyPrefix.Append("Unit");
            BuildKey(typeof(T));
            _keyPrefix.Append(":");
        }

        private void BuildKey(Type t)
        {
            _keyPrefix.Append(":");
            _keyPrefix.Append(t.Name);
            if (t.IsGenericType)
            {
                foreach (var arg in t.GetGenericArguments())
                {
                    BuildKey(arg);
                }
            }
        }
    }
}
