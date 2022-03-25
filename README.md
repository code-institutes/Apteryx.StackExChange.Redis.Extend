# 针对StackExChange.Redis的扩展，加入实体（String类型）并可设置过期时间
![Image text](https://github.com/code-institutes/Apteryx.StackExChange.Redis.Extend/blob/master/demo1.png)

# 使用方法一、

    //file: Account.cs
    public class Account: BaseRedisEntity
    {
        public string Name { get; set; }
    }

    //file: AppRedisContext.cs
    public class AppRedisContext : RedisDBProvider
    {
        public AppRedisContext(IOptionsMonitor<RedisOptions> options) : base(options) { }

        public IRedisCollection<Account> Accounts => Database.GetCollection<Account>();
    }

    static void Main(string[] args)
    {
        AppRedisContext redis = new AppRedisContext("xxx.xx.xx.248:6379,password=xxx,defaultDatabase=1");
        for (int i = 1; i <= 10; i++)
        {
            redis.Accounts.Add(new Account() { Name = "张三" + i }, TimeSpan.FromMinutes(1));
        }
        await redis.Accounts.AddAsync("key11", new Account() { Name = "张三11" }, TimeSpan.FromMinutes(1));
        var account1 = redis.Accounts.Find("key11");//支持key查询
        var account2 = redis.Accounts.FirstOrDefault(f => f.Name == "张三8");//注意此方法为遍历对比，只适用少量数据。
    }


# 使用方法二、

    //file: Account.cs
    public class Account: BaseRedisEntity
    {
        public string Name { get; set; }
    }

    //file: AppRedisContext.cs
    public class AppRedisContext : RedisDBProvider
    {
        public AppRedisContext(IOptionsMonitor<RedisOptions> options) : base(options) { }

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
            services.AddRedis<AppRedisContext>(options =>
            {
                //options.ConnectionString = Configuration.GetConnectionString("RedisConnection");
                options.ConnectionString = "xxx.xxx.xxx.xxx:6379,password = 123456,defaultDatabase = 0";
            });
            //............................
        }
    }
    
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private RedisEntity _redis = null;
        public ValuesController(AppRedisContext redis)
        {
            _redis = redis;
        }

        [HttpGet()]
        public IActionResult Get()
        {
            for (int i = 1; i <= 10; i++)
            {
                _redis.Accounts.Add(new Account() { Name = "张三" + i }, TimeSpan.FromMinutes(1));
            }
            _redis.Accounts.Add("key11", new Account() { Name = "张三11" }, TimeSpan.FromMinutes(1));
            var account1 = _redis.Accounts.Find("key11");//支持key查询
            var account2 = _redis.Accounts.FirstOrDefault(f => f.Name == "张三8");//注意此方法为遍历对比，只适用少量数据。

            return Ok(account1);
        }
    }
    
