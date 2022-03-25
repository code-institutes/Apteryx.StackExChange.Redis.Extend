using System.Text.Json;

namespace Apteryx.StackExChange.Redis.Extend
{
    public static class JsonHelper
    {
        public static string ToJson<T>(this T obj)
        {
            return JsonSerializer.Serialize(obj);
        }
        
        public static T FromJson<T>(this string s)
        {
            return JsonSerializer.Deserialize<T>(s);
        }
    }
}
