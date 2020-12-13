using System;
using System.Collections.Generic;
using System.Text;
using XExten.HttpFactory;
using XExten.HttpFactory.MultiInterface;
using System.Linq;
using XExten.XPlus;
using HtmlAgilityPack;
using AlmCore.Scrapy.MediaModel;
using AlmCore.SQLService;

namespace AlmCore.Scrapy
{
    public class Tencent
    {
        private const string BaseURL = "https://v.qq.com/x/search/?q={0}&cur={1}";

        public static List<MediaRoot> GetTencentSearch(string Keyword, Action<Exception> action = null)
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
                    roots.AddRange(LoadTencentSearch(html, action));
                }
                return roots;
            }, ex =>
            {
                action?.Invoke(ex);
                return null;
            });
        }

        private static List<MediaElements> GetTencentSearchDetail(string URL, string Name, Action<Exception> action = null)
        {
            return XPlusEx.XTry(() =>
             {
                 List<MediaElements> elements = new List<MediaElements>();
                 var Html = HttpMultiClient.HttpMulti.AddNode(URL).Build().RunString().FirstOrDefault();
                 HtmlDocument document = new HtmlDocument();
                 document.LoadHtml(Html);
                 var Data = document.DocumentNode.SelectNodes("//div[@class='wrapper_main']//div[@class='mod_episode']//a");
                 foreach (var item in Data)
                 {
                     string CollectStr = string.Empty;
                     int.TryParse(item.SelectSingleNode("span").InnerText, out int Collect);
                     if (Collect == 0)
                         CollectStr = "全集";
                     else if (Collect > 0 && Collect < 10)
                         CollectStr = $"第0{Collect}集";
                     else
                         CollectStr = $"第{Collect}集";
                     elements.Add(new MediaElements
                     {
                         PlayUrl = item.GetAttributeValue("href", ""),
                         Collect = CollectStr,
                         Names = Name
                     });
                 }
                 return elements;
             }, ex =>
             {
                 action?.Invoke(ex);
                 return null;
             });
        }

        private static List<MediaRoot> LoadTencentSearch(string html, Action<Exception> action = null)
        {
            return XPlusEx.XTry(() =>
             {
                 List<MediaRoot> roots = new List<MediaRoot>();
                 HtmlDocument document = new HtmlDocument();
                 document.LoadHtml(html);
                 var Doc = document.DocumentNode;
                 var All = Doc.SelectNodes("//div[@data-suggest][@data-id][@r-notemplate='true']");
                 if (All != null && All.Count != 0)
                 {
                     foreach (var item in All)
                     {
                         MediaRoot root = new MediaRoot()
                         {
                             Elements = new List<MediaElements>()
                         };
                         var infos = item.SelectSingleNode("div[@class='_infos']//h2[@class='result_title']/a");
                         var DetailPage = infos.GetAttributeValue("href", "");
                         var Type = infos.SelectSingleNode("span[@class='type']").InnerText;
                         var Nodes = infos.Descendants("em");
                         if (Nodes.Count() != 0)
                         {
                             root.Name = Nodes.FirstOrDefault().InnerText;
                             root.Cover = "http:" + item.SelectSingleNode("div[@class='_infos']//img[@class='figure_pic']").GetAttributeValue("src", "");
                             #region 电视剧
                             if (Type.Equals("电视剧"))
                             {
                                 var NewsHtml = item.SelectNodes("div[@class='_playlist']/div[@r-component='inline-teleplay']//div[@class='item item_fold']");
                                 if (NewsHtml != null && NewsHtml?.Count != 0)
                                 {
                                     root.Elements.AddRange(GetTencentSearchDetail(DetailPage, root.Name,action));
                                 }
                                 else
                                 {
                                     var ContentHtml = item.SelectNodes("div[@class='_playlist']/div[@r-component='inline-teleplay']//div[@class='item']/a");
                                     NullExten(root.Elements, ContentHtml, root.Name, () =>
                                     {
                                         foreach (var Content in ContentHtml)
                                         {
                                             string CollectStr = string.Empty;
                                             int.TryParse(Content.InnerText, out int Collect);
                                             if (Collect == 0)
                                                 CollectStr = "全集";
                                             else if (Collect > 0 && Collect < 10)
                                                 CollectStr = $"第0{Collect}集";
                                             else
                                                 CollectStr = $"第{Collect}集";
                                             root.Elements.Add(new MediaElements
                                             {
                                                 PlayUrl = Content.GetAttributeValue("href", ""),
                                                 Collect = CollectStr,
                                                 Names = root.Name
                                             });
                                         }
                                     });
                                 }
                             }
                             #endregion
                             #region 电影
                             if (Type.Equals("电影"))
                             {
                                 var ContentHtml = item.SelectNodes("div[@class='_playlist']/div[@class='result_btn_line']/a[@_stat]");
                                 NullExten(root.Elements, ContentHtml, root.Name, () =>
                                 {
                                     foreach (var Content in ContentHtml)
                                     {
                                         root.Elements.Add(new MediaElements
                                         {
                                             PlayUrl = Content.GetAttributeValue("href", ""),
                                             Collect = Content.Descendants("span").FirstOrDefault().InnerText,
                                             Names = root.Name
                                         });
                                     }
                                 });
                             }
                             #endregion
                             #region 动漫
                             if (Type.Equals("动漫"))
                             {
                                 var ContentHtml = item.SelectNodes("div[@class='_playlist']//div[@r-component='inline-telelist']//div[@class='item']/a");
                                 NullExten(root.Elements, ContentHtml, root.Name, () =>
                                 {
                                     foreach (var Content in ContentHtml)
                                     {
                                         string CollectStr = string.Empty;
                                         int.TryParse(Content.InnerText, out int Collect);
                                         if (Collect == 0)
                                             CollectStr = "全集";
                                         else if (Collect > 0 && Collect < 10)
                                             CollectStr = $"第0{Collect}集";
                                         else
                                             CollectStr = $"第{Collect}集";
                                         root.Elements.Add(new MediaElements
                                         {
                                             PlayUrl = Content.GetAttributeValue("href", ""),
                                             Collect = CollectStr,
                                             Names = root.Name
                                         });
                                     }
                                 });
                             }
                             #endregion
                             #region 综艺
                             if (Type.Equals("综艺"))
                             {
                                 var ContentHtml = item.SelectNodes("div[@class='_playlist']//div[@r-component='inline-series-list']//div[@class='item']/a");
                                 NullExten(root.Elements, ContentHtml, root.Name, () =>
                                 {
                                     foreach (var Content in ContentHtml)
                                     {
                                         root.Elements.Add(new MediaElements
                                         {
                                             PlayUrl = Content.GetAttributeValue("href", ""),
                                             Collect = Content.InnerText,
                                             Names = root.Name
                                         });
                                     }
                                 });
                             }
                             #endregion
                             roots.Add(root);
                         }
                     }
                 }
                 return roots;
             }, ex =>
             {
                 action?.Invoke(ex);
                 return null;
             });
        }

        private static void NullExten(List<MediaElements> elements, HtmlNodeCollection ContentHtml, string Name, Action action)
        {
            if (ContentHtml != null && ContentHtml?.Count != 0)
            {
                action.Invoke();
            }
            else
            {
                elements.Add(new MediaElements
                {
                    PlayUrl = null,
                    Collect = "暂无资源",
                    Names = Name
                });
            }
        }
    }
}
