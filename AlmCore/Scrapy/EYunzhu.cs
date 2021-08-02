using AlmCore.Scrapy.EYunzhuModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XExten.HttpFactory;
using XExten.XCore;
using XExten.XPlus;

namespace AlmCore.Scrapy
{
    public class EYunzhu
    {
        private const string BaseURL = "https://api.eyunzhu.com/api/vatfs/resource_site_collect/";
        private const string Search = "search?kw={0}&page={1}&per_page=10";
        private const string Detail = "getVDetail?vid={0}";

        public static SearchRoots GetMovieSearch(string keyword, int page = 1, Action<Exception> action = null)
        {
            return RetryException.DoRetry(() =>
            {
                return XPlusEx.XTry(() =>
                {
                    var URL = BaseURL + string.Format(Search, keyword, page);
                    var JsonData = HttpMultiClient.HttpMulti.Headers(Extension.Headers).AddNode(URL).Build(60,true).RunString();
                    return JsonData.FirstOrDefault().ToModel<JObject>().SelectToken("data").ToString().ToModel<SearchRoots>();
                }, ex =>
                {
                    LogFactory.WriteLog(ex);
                    action?.Invoke(ex);
                    return null;
                });
            });
        }

        public static DetailRoots GetMovieDetail(int Id, Action<Exception> action = null) {
            return RetryException.DoRetry(() =>
            {
                return XPlusEx.XTry(() =>
                {
                    var URL = BaseURL + string.Format(Detail, Id);
                    var JsonData = HttpMultiClient.HttpMulti.Headers(Extension.Headers).AddNode(URL).Build(60, true).RunString();
                    return JsonData.FirstOrDefault().ToModel<JObject>().SelectToken("data").ToString().ToModel<DetailRoots>();
                }, ex =>
                {
                    LogFactory.WriteLog(ex);
                    action?.Invoke(ex);
                    return null;
                });
            });
        }
    }
}
