using Apteryx.StackExChange.Redis.Extend.App.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Apteryx.StackExChange.Redis.Extend.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TestController> _logger;
        private readonly AppRedisContext _redis;

        public TestController(ILogger<TestController> logger, AppRedisContext redis)
        {
            _logger = logger;
            _redis = redis;
        }

        [HttpGet("fun1")]
        public async Task<IActionResult> Fun1()
        {
            AppRedisContext redis = new AppRedisContext("192.168.1.55:6379,password=12345678,connectTimeout=1000,connectRetry=2,syncTimeout=10000,defaultDatabase=1");
            for (int i = 1; i <= 10; i++)
            {
                redis.Accounts.Add(new Account() { Name = "张三" + i }, DateTime.Now.AddMinutes(1));
            }
            await redis.Accounts.AddAsync("key11", new Account() { Name = "张三11" }, TimeSpan.FromMinutes(1));
            var account1 = redis.Accounts.Find("key11");//支持key查询
            var account2 = redis.Accounts.FirstOrDefault(f => f.Name == "张三8");//注意此方法为遍历对比，只适用少量数据。
            return Ok(Content("success"));
        }

        [HttpGet("fun2")]
        public async Task<IActionResult> Fun2()
        {
            for (int i = 1; i <= 10000; i++)
            {
                _redis.Accounts.AddAsync(new Account() { Name = "张三" + i }, TimeSpan.FromMinutes(1));
            }
            _redis.Accounts.Add("key11", new Account() { Name = "张三11" }, TimeSpan.FromMinutes(1));
            var account1 = _redis.Accounts.Find("key11");//支持key查询
            //var account2 = _redis.Accounts.FirstOrDefault(f => f.Name == "张三5000");//注意此方法为遍历对比，只适用少量数据。

            return Ok(Content("success"));
        }
    }
}
