using AlmCore.Scrapy.IQiyiModel;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XExten.HttpFactory;
using XExten.XCore;
using XExten.XPlus;

namespace AlmCore.Scrapy
{
    public class IQiyi
    {
        private const string BaseURL = "https://so.iqiyi.com/so/q_{0}_ctg__t_0_page_{1}_p_1_qc_0_rd__site_iqiyi_m_1_bitrate__af_0";
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
                if (MovieType!=null)
                {
                    foreach (var item in MovieType)
                    {
                        IQiyiRoot root = new IQiyiRoot
                        {
                            Elements = new List<IQiyiElements>()
                        };
                        root.Elements.Add(new IQiyiElements
                        {
                            Collect = "全集",
                            PlayUrl = "https:" + item.GetAttributeValue("href", "")
                        });
                        var Img = item.Descendants("img").FirstOrDefault();
                        root.Name = Img.GetAttributeValue("alt", "");
                        root.Cover = "http:" + Img.GetAttributeValue("src", "");
                        roots.Add(root);
                    }
                }
                #endregion
                #region 获取剧集类
                var TVSeries = Doc.SelectNodes("//div[@desc='剧集类']");
                if (TVSeries!=null)
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
                            root.Elements.Add(new IQiyiElements
                            {
                                Collect = target.GetAttributeValue("title", ""),
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
    }
}
