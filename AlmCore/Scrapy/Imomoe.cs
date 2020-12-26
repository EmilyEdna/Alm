using AlmCore.Scrapy.ImomoeModel;
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
    public class Imomoe
    {
        private const string BaseURL = "http://www.yhdm.io";
        private const string Search = "/search/{0}/?page={1}";

        #region 获取远程数据
        public static BangumiRoot GetBangumi(object Keyword, int Page = 1, Action<Exception> action = null)
        {
            return RetryException.DoRetry(() =>
             {
                 if (Keyword.GetType() == typeof(string))
                 {
                     //每页20条数据 关键字查询
                     var data = HttpMultiClient.HttpMulti.Headers(Extension.Headers).AddNode(BaseURL + string.Format(Search, Keyword, Page)).Build().RunString().FirstOrDefault();
                     return LoadSearchPage(data, 20, action);
                 }
                 else
                 {
                     //每页15条数据 年份查询
                     var host = (Page == 0 || Page == 1) ? $"/{Keyword}" : $"/{Keyword}/{Page}.html";
                     var data = HttpMultiClient.HttpMulti.Headers(Extension.Headers).AddNode(BaseURL + host).Build().RunString().FirstOrDefault();
                     return LoadSearchPage(data, 15, action);
                 }
             });
        }

        public static BangumiDetailRoot GetBangumiPage(string Route, Action<Exception> action = null)
        {
            return RetryException.DoRetry(() =>
            {
                var data = HttpMultiClient.HttpMulti.Headers(Extension.Headers).AddNode(BaseURL + Route).Build().RunString().FirstOrDefault();
                return LoadPlayPage(data, action);
            });
        }

        public static string GetVedio(string PlayHtml, Action<Exception> action = null)
        {
            return RetryException.DoRetry(() =>
            {
                var data = HttpMultiClient.HttpMulti.Headers(Extension.Headers).AddNode(BaseURL + PlayHtml).Build().RunString().FirstOrDefault();
                return LoadBangumi(data, action);
            });
        }

        #region 解析数据
        private static BangumiRoot LoadSearchPage(string html, double pageNo = 20, Action<Exception> action = null)
        {
            return XPlusEx.XTry(() =>
            {
                BangumiRoot roots = new BangumiRoot
                {
                    Elements = new List<BangumiElements>()
                };
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var data = document.DocumentNode.SelectNodes("//div[@class='lpic']//li");
                if (data.Count != 0)
                {
                    foreach (var item in data)
                    {
                        roots.Elements.Add(new BangumiElements
                        {
                            Conver = item.Descendants("img").FirstOrDefault().GetAttributeValue("src", ""),
                            DetailPage = item.Descendants("a").FirstOrDefault().GetAttributeValue("href", ""),
                            BangumiName = item.Descendants("img").FirstOrDefault().GetAttributeValue("alt", "")
                        });
                    }
                }
                var page = document.DocumentNode.SelectSingleNode("//div[@class='pages']");
                if (page != null)
                {
                    int.TryParse(Regex.Match(page.Descendants("a").FirstOrDefault().InnerText, "\\d+").Value, out int Total);
                    roots.Total = Total;
                    roots.TotalPage = Convert.ToInt32(Math.Ceiling(Total / pageNo));
                }
                else
                {
                    roots.Total = 1;
                    roots.TotalPage = 1;
                }
                return roots;
            }, ex =>
            {
                action?.Invoke(ex);
                return null;
            });
        }
        private static BangumiDetailRoot LoadPlayPage(string html, Action<Exception> action = null)
        {
            return XPlusEx.XTry(() =>
            {
                BangumiDetailRoot roots = new BangumiDetailRoot
                {
                    Elements = new List<BangumiDetailElements>()
                };
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var Nodes = document.DocumentNode;
                roots.Conver = Nodes.SelectSingleNode("//div[@class='thumb l']//img").GetAttributeValue("src", "");
                roots.Description = Nodes.SelectSingleNode("//div[@class='info']").InnerText.Replace("\r\n", "");
                var data = Nodes.SelectNodes("//div[@class='movurl']//li");
                if (data.Count != 0)
                {
                    foreach (var item in data)
                    {
                        roots.Elements.Add(new BangumiDetailElements
                        {
                            PlayPage = item.Descendants("a").FirstOrDefault().GetAttributeValue("href", ""),
                            Collection = item.Descendants("a").FirstOrDefault().InnerText.Replace("'", "")
                        }); ;
                    }
                }
                return roots;
            }, ex =>
            {
                LogFactory.WriteLog(ex);
                action?.Invoke(ex);
                return null;
            });
        }
        private static string LoadBangumi(string html, Action<Exception> action = null)
        {
            return XPlusEx.XTry(() =>
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var node = document.DocumentNode.SelectSingleNode("//div[@class='play']//div[@data-vid]");
                var URL = node.GetAttributeValue("data-vid", "$").Replace("$mp4", "");
                if (URL.Contains(".mp4") || URL.Contains("m3u8"))
                    return URL;
                var res = HttpMultiClient.HttpMulti.AddNode(URL).Build().RunString().FirstOrDefault();
                if (!res.IsNullOrEmpty())
                    return Regex.Match(res, "http(.*)").Value;
                return null;
            }, ex =>
            {
                LogFactory.WriteLog(ex);
                action?.Invoke(ex);
                return null;
            });
        }
        #endregion
        #endregion
    }
}
