using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using apteryx.stackexchange.redis.extend.Entities;
using ServiceStack;
using StackExchange.Redis;

namespace Apteryx.StackExChange.Redis.Extend.UnitHandlers
{
    public class StringListHandler<T> : IUnit<T>
        where T : BaseRedisEntity
    {
        private bool m_disposed;
        private readonly IDatabase db;
        private StringBuilder _keyPrefix = new StringBuilder();
        public string KeyPrefix => _keyPrefix.ToString();
        public string Key { get; protected set; }
        public Func<string, string> DKey => (key) => { return string.Format("{0}{1}", KeyPrefix, key); };

        public StringListHandler(IDatabase database)
        {
            this.db = database;
            BuildKeyPrefix();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public long Append(T value, CommandFlags flags = CommandFlags.None)
        {
            string key = GetKey();
            return db.StringAppend(key, value.ToJson(), flags);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Task<long> AppendAsync(T value, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() => { return Append(value, flags); });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public long Append(T value, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            string key = GetKey();
            var vl = db.StringAppend(key, value.ToJson(), flags);
            var vb = db.KeyExpire(key, expiry);
            if (vb)
            {
                return vl;
            }
            else
            {
                db.KeyDelete(key, flags);
            }

            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Task<long> AppendAsync(T value, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() => { return Append(value, expiry, flags); });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public long Append(T value, DateTime? expiry, CommandFlags flags = CommandFlags.None)
        {
            string key = GetKey();
            var vl = db.StringAppend(key, value.ToJson(), flags);
            var vb = db.KeyExpire(key, expiry);
            if (vb)
            {
                return vl;
            }
            else
            {
                db.KeyDelete(key, flags);
            }

            return 0;
        }
        public Task<long> AppendAsync(T value, DateTime? expiry, CommandFlags flags = CommandFlags.None)
        {
            return Task.Run(() => { return Append(value, expiry, flags); });
        }
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            foreach (var ep in db.Multiplexer.GetEndPoints())
            {
                var server = db.Multiplexer.GetServer(ep);
                var keys = server.Keys(pattern: KeyPrefix + "*", database: db.Database);
                foreach (var k in keys)
                {
                    this.Key = k;
                    yield return db.StringGet(k).ToString().FromJson<T>();
                }
            }
        }

        public T FirstOrDefault(Func<T, bool> predicate)
        {
            foreach (var ep in db.Multiplexer.GetEndPoints())
            {
                var server = db.Multiplexer.GetServer(ep);
                var keys = server.Keys(pattern: KeyPrefix + "*", database: db.Database);
                foreach (var k in keys)
                {
                    this.Key = k;
                    var obj = db.StringGet(k).ToString().FromJson<T>();
                    if (predicate.Invoke(obj))
                        return obj;

                }
            }

            return default(T);
        }

        public bool Delete(T obj)
        {
            var key = DKey(obj._key);
            return db.KeyDelete(key);
        }

        public Task<bool> DeleteAsync(T obj)
        {
            var key = DKey(obj._key);
            return db.KeyDeleteAsync(key);
        }

        public bool Delete(Func<T, bool> predicate)
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

        public Task<bool> DeleteAsync(Func<T, bool> predicate)
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

            return Task.Run(() => { return false; });
        }

        /// <summary>
        /// 删除所有
        /// </summary>
        /// <returns></returns>
        public long DeleletAll()
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task DeleletAllAsync()
        {
            return Task.Run(() => { return DeleletAll(); });
        }

        private string GetKey()
        {
            this.Key = string.Format("{0}{1}", KeyPrefix, Guid.NewGuid());
            return this.Key;
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                    // Release managed resources

                }
                // Release unmanaged resources

                m_disposed = true;
            }
        }

        ~StringListHandler()
        {
            Dispose(false);
        }
    }
}
