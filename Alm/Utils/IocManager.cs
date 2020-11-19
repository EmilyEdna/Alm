using System.Collections.Concurrent;

namespace Alm.Utils
{
    public class IocManager
    {
        private static readonly ConcurrentDictionary<string, object> Cache = new ConcurrentDictionary<string, object>();
        public static void SetCache(string key, object value)
        {
            if (!Cache.ContainsKey(key))
                Cache.TryAdd(key, value);
        }
        public static T GetCache<T>(string key) 
        {
            if (!Cache.ContainsKey(key)) return default;
            Cache.TryGetValue(key, out object obj);
            return (T)obj;
        }
    }
}
