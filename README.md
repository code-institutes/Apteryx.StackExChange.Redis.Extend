# 针对StackExChange.Redis的扩展，加入实体（String类型）并可设置过期时间
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
    
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRedis<RedisEntity>(options =>
            {
                options.ConnectionString = "xxx.xxx.xxx.xxx:6379,password = 123456,defaultDatabase = 0";
            });
            //............................
        }
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
            for (int i = 1; i <= 10; i++)
            {
                db.Accounts.AddAsync(new Account() {Name = "张三" + i}, TimeSpan.FromSeconds(20));
            }
            
            db.Accounts.AddAsync("key11", new Account() {Name = "张三11"}, TimeSpan.FromSeconds(20));
            var account1 = db.Accounts.Find("key11");//支持key查询
            var account2 = db.Accounts.FirstOrDefault(f => f.Name == "张三8");//注意此方法为遍历对比，只适用少量数据。
            return Ok(account1);
        }
    }
    
