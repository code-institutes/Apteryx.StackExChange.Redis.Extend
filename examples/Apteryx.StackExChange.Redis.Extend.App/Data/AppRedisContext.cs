using Microsoft.Extensions.Options;

namespace Apteryx.StackExChange.Redis.Extend.App.Data
{
    public class AppRedisContext : RedisDBProvider
    {
        public AppRedisContext(IOptionsMonitor<RedisOptions> options) : base(options) { }
        public AppRedisContext(string conn):base(conn) { }
        public IRedisCollection<Account> Accounts => Database.GetCollection<Account>();
    }
}
