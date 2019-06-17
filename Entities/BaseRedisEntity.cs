namespace apteryx.stackexchange.redis.extend.Entities
{
    public abstract class BaseRedisEntity:IRedisEntity
    {
        public string _key { get; protected set; }
    }
}
