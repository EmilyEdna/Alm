using AlmCore.Scrapy.IQiyiModel;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XExten.HttpFactory;
using XExten.XCore;
using XExten.XPlus;

namespace AlmCore.Scrapy
{
    public class IQiyi
    {
        private const string BaseURL = "https://so.iqiyi.com/so/q_{0}_ctg__t_0_page_{1}_p_1_qc_0_rd__site_iqiyi_m_1_bitrate__af_0";
        private const string Resolv = "https://vip.52jiexi.top/?url=";
        /// <summary>
        /// 爱奇艺查询
        /// </summary>
        /// <param name="Keyword"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static List<IQiyiRoot> GetIQiyiSearch(string Keyword, Action<Exception> action = null)
        {
            List<IQiyiRoot> roots = new List<IQiyiRoot>();
            var htmls = HttpMultiClient.HttpMulti
                 .AddNode(string.Format(BaseURL, Keyword, 1))
                 .AddNode(string.Format(BaseURL, Keyword, 2))
                 .AddNode(string.Format(BaseURL, Keyword, 3))
                 .Build().RunString();
            foreach (var html in htmls)
            {
                roots.AddRange(LoadIQiyiSearch(html, action));
            }
            return roots;
        }

        private static List<IQiyiRoot> LoadIQiyiSearch(string html, Action<Exception> action = null)
        {
            return XPlusEx.XTry(() =>
            {
                List<IQiyiRoot> roots = new List<IQiyiRoot>();
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var Doc = document.DocumentNode;
                #region 获取电影类
                var MovieType = Doc.SelectNodes("//div[@desc='长视频(电影)类']//a[@class='qy-mod-link']");
                if (MovieType != null)
                {
                    foreach (var item in MovieType)
                    {
                        IQiyiRoot root = new IQiyiRoot
                        {
                            Elements = new List<IQiyiElements>()
                        };
                        var Img = item.Descendants("img").FirstOrDefault();
                        root.Name = Img.GetAttributeValue("alt", "");
                        root.Cover = "http:" + Img.GetAttributeValue("src", "");
                        root.Elements.Add(new IQiyiElements
                        {
                            Names = root.Name,
                            Collect = "全集",
                            PlayUrl = "https:" + item.GetAttributeValue("href", "")
                        });
                        roots.Add(root);
                    }
                }
                #endregion
                #region 获取剧集类
                var TVSeries = Doc.SelectNodes("//div[@desc='剧集类']");
                if (TVSeries != null)
                {
                    foreach (var TV in TVSeries)
                    {
                        IQiyiRoot root = new IQiyiRoot
                        {
                            Elements = new List<IQiyiElements>()
                        };
                        var Img = TV.SelectSingleNode("div[@class='result-figure']//img");
                        root.Name = Img.GetAttributeValue("alt", "");
                        root.Cover = "http:" + Img.GetAttributeValue("src", "");
                        TV.SelectSingleNode("div[@class='result-right']//ul[@style='display:none;']").Descendants("li").ToEachs(item =>
                        {
                            var target = item.Element("a");
                            string CollectStr = string.Empty;
                            int.TryParse(Regex.Match(target.GetAttributeValue("title", ""), "\\d+").Value, out int Collect);
                            if (Collect == 0)
                                CollectStr = "全集";
                            else if (Collect > 0 && Collect < 10)
                                CollectStr = $"第0{Collect}集";
                            else
                                CollectStr = $"第{Collect}集";
                            root.Elements.Add(new IQiyiElements
                            {
                                Names= root.Name,
                                Collect = CollectStr,
                                PlayUrl = "https:" + target.GetAttributeValue("href", "")
                            });
                        });
                        roots.Add(root);
                    }
                }
                #endregion
                return roots;
            }, ex =>
            {
                action?.Invoke(ex);
                return null;
            });
        }

        /// <summary>
        /// 获取视频流
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static string ResolvURL(string Url)
        {
            var data = HttpMultiClient.HttpMulti.AddNode(Resolv + Url).Build().RunString();
            var Fitlers = Regex.Match(data.FirstOrDefault(), "=\\s\\S(http).*\"").Value;
            return Regex.Match(Fitlers, "http.*\"").Value.Replace("\"", "");
        }
    }
}
