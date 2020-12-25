using AlmCore.Scrapy.MediaModel;
using AlmCore.SQLService;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XExten.HttpFactory;
using XExten.HttpFactory.MultiInterface;
using XExten.XCore;
using XExten.XPlus;

namespace AlmCore.Scrapy
{
    public class IQiyi
    {
        private const string BaseURL = "https://so.iqiyi.com/so/q_{0}_ctg__t_0_page_{1}_p_1_qc_0_rd__site_iqiyi_m_1_bitrate__af_0";
        /// <summary>
        /// 爱奇艺查询
        /// </summary>
        /// <param name="Keyword"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static List<MediaRoot> GetIQiyiSearch(string Keyword, Action<Exception> action = null)
        {
            return RetryException.DoRetry(() =>
             {
                 return XPlusEx.XTry(() =>
                  {
                      List<MediaRoot> roots = new List<MediaRoot>();
                      int Size = CommonLogic.Logic.GetOptions().Select(t => t.OptionPage).FirstOrDefault();
                      INode Node = HttpMultiClient.HttpMulti.AddNode(string.Format(BaseURL, Keyword, 1));
                      for (int i = 2; i <= Size; i++)
                      {
                          Node = Node.AddNode(string.Format(BaseURL, Keyword, i));
                      }
                      var htmls = Node.Build().RunString();
                      foreach (var html in htmls)
                      {
                          roots.AddRange(LoadIQiyiSearch(html, action));
                      }
                      return roots;

                  }, ex =>
                  {
                      LogFactory.WriteLog(ex);
                      action?.Invoke(ex);
                      return null;
                  });
             });
        }

        private static List<MediaRoot> LoadIQiyiSearch(string html, Action<Exception> action = null)
        {
            return XPlusEx.XTry(() =>
            {
                List<MediaRoot> roots = new List<MediaRoot>();
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var Doc = document.DocumentNode;
                #region 获取电影类
                var MovieType = Doc.SelectNodes("//div[@desc='长视频(电影)类']//a[@class='qy-mod-link']");
                if (MovieType != null)
                {
                    foreach (var item in MovieType)
                    {
                        MediaRoot root = new MediaRoot
                        {
                            Elements = new List<MediaElements>()
                        };
                        var Img = item.Descendants("img").FirstOrDefault();
                        root.Name = Img.GetAttributeValue("alt", "");
                        root.Cover = "http:" + Img.GetAttributeValue("src", "");
                        root.Elements.Add(new MediaElements
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
                        MediaRoot root = new MediaRoot
                        {
                            Elements = new List<MediaElements>()
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
                            root.Elements.Add(new MediaElements
                            {
                                Names = root.Name,
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
                LogFactory.WriteLog(ex);
                action?.Invoke(ex);
                return null;
            });
        }
    }
}
