using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Apteryx.StackExChange.Redis.Extend
{
    public class RedisCollection<T> : IRedisCollection<T>
        where T : BaseRedisEntity
    {
        private readonly IDatabase _db;
        private StringBuilder _keyPrefix = new StringBuilder();
        public string KeyPrefix { get; private set; }
        public Func<string, string> DKey => (key) => { return KeyPrefix + key; };

        public RedisCollection(IDatabase database)
        {
            this._db = database;
            BuildKeyPrefix();
        }

        public T Find(string key)
        {
            key = DKey(key);
            return _db.StringGet(key).ToString().FromJson<T>();
        }

        public async Task<T> FindAsync(string key)
        {
            key = DKey(key);
            return (await _db.StringGetAsync(key)).ToString().FromJson<T>();
        }

        public T FirstOrDefault(Func<T, bool> predicate)
        {
            foreach (var ep in _db.Multiplexer.GetEndPoints())
            {
                var server = _db.Multiplexer.GetServer(ep);
                var keys = server.Keys(pattern: KeyPrefix + "*", database: _db.Database);
                foreach (var k in keys)
                {
                    var obj = _db.StringGet(k).ToString().FromJson<T>();
                    if (predicate.Invoke(obj))
                        return obj;
                }
            }

            return default(T);
        }

        public Task<T> FirstOrDefaultAsync(Func<T, bool> predicate)
        {
            return Task.Run(()=> FirstOrDefault(predicate));
        }

        public IEnumerable<T> FindAll()
        {
            foreach (var ep in _db.Multiplexer.GetEndPoints())
            {
                var server = _db.Multiplexer.GetServer(ep);
                var keys = server.Keys(pattern: KeyPrefix + "*", database: _db.Database);
                foreach (var k in keys)
                {
                    yield return _db.StringGet(k).ToString().FromJson<T>();
                }
            }
        }

        public bool Remove(T obj)
        {
            var key = DKey(obj._key);
            return _db.KeyDelete(key);
        }

        public Task<bool> RemoveAsync(T obj)
        {
            var key = DKey(obj._key);
            return _db.KeyDeleteAsync(key);
        }

        public RemoveResult Remove(Func<T, bool> predicate)
        {
            long count = 0;
            foreach (var ep in _db.Multiplexer.GetEndPoints())
            {
                var server = _db.Multiplexer.GetServer(ep);
                var keys = server.Keys(pattern: KeyPrefix + "*", database: _db.Database);
                foreach (var key in keys)
                {
                    var obj = _db.StringGet(key).ToString().FromJson<T>();
                    if (predicate.Invoke(obj))
                    {
                        if (_db.KeyDelete(key))
                            ++count;
                    }
                }
            }

            return new RemoveResult(count);
        }

        public Task<RemoveResult> RemoveAsync(Func<T, bool> predicate)
        {
            return Task.Run(() => Remove(predicate));
        }

        public RemoveResult RemoveAll()
        {
            long count = 0;
            foreach (var ep in _db.Multiplexer.GetEndPoints())
            {
                var server = _db.Multiplexer.GetServer(ep);
                var keys = server.Keys(pattern: KeyPrefix + "*", database: _db.Database);
                foreach (var k in keys)
                {
                    if (_db.KeyDelete(k))
                        ++count;
                }
            }

            return new RemoveResult(count);
        }

        public Task<RemoveResult> RemoveAllAsync()
        {
            return Task.Run(() => RemoveAll());
        }

        public bool Add(T value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Add(value._key, value, expiry, when, flags);
        }

        public bool Add(T value, DateTime? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Add(value._key, value, expiry, when, flags);
        }

        public bool Add(string key, T value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            key = DKey(key);
            return _db.StringSet(key, value.ToJson(), expiry, when, flags);
        }

        public bool Add(string key, T value, DateTime? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            TimeSpan? ts = null;
            if (expiry != null)
                ts = expiry - DateTime.Now;
            return Add(key, value, ts, when, flags);
        }

        public Task<bool> AddAsync(T value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return AddAsync(value._key, value, expiry, when, flags);
        }

        public Task<bool> AddAsync(T value, DateTime? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return AddAsync(value._key, value, expiry, when, flags);
        }

        public Task<bool> AddAsync(string key, T value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            key = DKey(key);
            return _db.StringSetAsync(key, value.ToJson(), expiry, when, flags);
        }

        public Task<bool> AddAsync(string key, T value, DateTime? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            TimeSpan? ts = null;
            if (expiry != null)
                ts = expiry - DateTime.Now;
            return AddAsync(value._key, value, ts, when, flags);
        }

        public void AddRange(IEnumerable<T> values, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            foreach (var value in values)
                Add(value, expiry, when, flags);
        }

        public void AddRange(IEnumerable<T> values, DateTime? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            TimeSpan? ts = null;
            if (expiry != null)
                ts = expiry - DateTime.Now;
            AddRange(values, ts, when, flags);
        }

        public Task AddRangeAsync(IEnumerable<T> values, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() => AddRange(values, expiry, when, flags));
        }

        public Task AddRangeAsync(IEnumerable<T> values, DateTime? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            TimeSpan? ts = null;
            if (expiry != null)
                ts = expiry - DateTime.Now;
            return AddRangeAsync(values, ts, when, flags);
        }

        private void BuildKeyPrefix()
        {
            _keyPrefix.Append("Unit");
            BuildKey(typeof(T));
            _keyPrefix.Append(":");

            this.KeyPrefix = _keyPrefix.ToString();
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
