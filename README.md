# 针对StackExChange.Redis的扩展，加入StringList并可设置过期时间
![Image text](https://github.com/gitapteryx/Apteryx.StackExChange.Redis.Extend/blob/master/Sample/resource/demo1.png)

# 使用方法一、

    public class Account
    {
        public string Name { get; set; }
    }

    public class RedisEntity:RedisDBProvider
    {
        public RedisEntity(string conn) : base(conn) { }

        public IRedisCollection<Account> Accounts
        {
            get { return _database.GetCollection<Account>(); }
        }
    }

    static void Main(string[] args)
    {
        RedisEntity db = new RedisEntity("xxx.xx.xx.248:6379,password=xxx,defaultDatabase=1");
        for (int i = 1; i <= 10; i++)
        {
            db.Accounts.AddAsync(new Account() {Name = "张三" + i}, TimeSpan.FromSeconds(20));
        }
    }

# 使用方法二、
    public class Account
    {
        public string Name { get; set; }
    }
    
    static void Main(string[] args)
    {
        var redisClient = ConnectionMultiplexer.Connect("xxx.xx.xx.248:6379,password=xxx,defaultDatabase=0");
        var db = redisClient.GetDatabase();
        using (var strlist = db.AsClientType<Account>())
        {
            for (int i = 1; i <= 100; i++)
            {
                strlist.Append(new Account() { Name = "张三" + i }, TimeSpan.FromMinutes(1));
            }
        }
    }
