using AlmCore.Scrapy.KonachanModel;
using AlmCore.SQLModel.Konachans;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XExten.HttpFactory;
using XExten.XCore;
using XExten.XPlus;

namespace AlmCore.Scrapy
{
    public class Konachan
    {
        #region 获取远程数据
        private const string BaseURL = "https://konachan.com/";
        private const string Tag = "tag.xml?order=date&limit={0}";
        private const string Home = "post.xml?page={0}&limit=15";

        public static ImageRoot GetImage(int Page = 1, string Tag = "", Action<Exception> action = null)
        {
            return RetryException.DoRetry(() =>
            {
                return XPlusEx.XTry(() =>
                {
                    var Hosts = string.Format(Home, Page);
                    if (!Tag.IsNullOrEmpty())
                        Hosts += $"&tags={Tag}";
                    var XmlData = HttpMultiClient.HttpMulti.Headers(Extension.Headers).AddNode(BaseURL + Hosts, UseCache: true).Build().CacheTime().RunString();
                    return XPlusEx.XmlDeserialize<ImageRoot>(XmlData.FirstOrDefault());
                }, ex =>
                {
                    action?.Invoke(ex);
                    return null;
                });
            });
        }
        public static TagRoot GetTag(int Page = 0, Action<Exception> action = null)
        {
            return RetryException.DoRetry(() =>
            {
                return XPlusEx.XTry(() =>
                {
                    var XmlData = HttpMultiClient.HttpMulti.Headers(Extension.Headers).AddNode(BaseURL + string.Format(Tag, Page)).Build().RunString();
                    return XPlusEx.XmlDeserialize<TagRoot>(XmlData.FirstOrDefault());
                }, ex =>
                {
                    LogFactory.WriteLog(ex);
                    action?.Invoke(ex);
                    return null;
                });
            });

        }
        #endregion

        #region 获取本地数据
        /// <summary>
        /// 初始化标签
        /// </summary>
        public static void InitTags()
        {
            var data = SQLContext.Lite.Queryable<Tags>().OrderBy(t => t.Id, OrderByType.Desc).First();
            if (data == null)
            {
                var res = GetTag().Elements.ToAutoMapper<TagElements, Tags>();
                SQLContext.Lite.Insertable(res).ExecuteCommand();
            }
            else
            {
                var res = GetTag(20).Elements.ToAutoMapper<TagElements, Tags>().Where(t => t.Id > data.Id).ToList();
                if (res.Count != 0)
                    SQLContext.Lite.Insertable(res).ExecuteCommand();
            }
        }
        #endregion
    }
}
