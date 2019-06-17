namespace Apteryx.StackExChange.Redis.Extend.UnitHandlers
{
    public interface IRedisDatabase
    {
        IRedisCollection<T> GetCollection<T>() where T : class;
    }
}
