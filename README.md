# 针对StackExChange.Redis的扩展，加入StringList并可设置过期时间
![Image text](https://github.com/code-institutes/Apteryx.StackExChange.Redis.Extend/blob/master/demo1.png)

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
# 使用方法三、
    public class Account
    {
        public string Name { get; set; }
    }

    public class RedisEntity : RedisDBProvider
    {
        public RedisEntity(IOptionsMonitor<RedisOptions> options) : base(options) { }

        public IRedisCollection<Account> Accounts => Database.GetCollection<Account>();
    }
    
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private RedisEntity db = null;
        public ValuesController(IRedisService redis)
        {
            db = redis as RedisEntity;
        }
        // GET api/values
        [HttpGet()]
        public IActionResult Get()
        {
            return Ok(db.Accounts.Find("50").ToJson());
        }
    }
    
