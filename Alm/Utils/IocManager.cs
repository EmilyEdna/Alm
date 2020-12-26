using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

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
        /// <summary>
        /// 获取子控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<T> GetChildObjects<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (((T)child).Name == name || string.IsNullOrEmpty(name)))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, ""));//指定集合的元素添加到List队尾  
            }
            return childList;
        }
    }
}
