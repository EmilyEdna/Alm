﻿using AlmCore.Scrapy.GithubModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XExten.HttpFactory;
using XExten.XCore;
using XExten.XPlus;

namespace AlmCore.Scrapy
{
    public class Github
    {
        private const string BaseURL = "https://raw.githubusercontent.com/EmilyEdna/Alm/master/Configs/";
        private const string SupportURL = "SupportList.json";
        private const string DeveloperURL = "DeveloperList.json";
        private const string VersionURL = "Version.txt";
        /// <summary>
        /// 获取赞助者
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static SupportRoot GetSupport(Action<Exception> action = null)
        {
            return RetryException.DoRetry(() =>
            {
                return XPlusEx.XTry(() =>
                {
                    var JsonData = HttpMultiClient.HttpMulti.AddNode(BaseURL + SupportURL, UseCache: true).Build().CacheTime().RunString();
                    return JsonData.FirstOrDefault().ToModel<SupportRoot>();
                }, ex =>
                {
                    action?.Invoke(ex);
                    return null;
                });
            });
        }
        /// <summary>
        /// 获取开发记录
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static List<DeveloperRoot> GetDevelopers(Action<Exception> action = null)
        {
            return RetryException.DoRetry(() =>
            {
                return XPlusEx.XTry(() =>
                {
                    var JsonData = HttpMultiClient.HttpMulti.AddNode(BaseURL + DeveloperURL, UseCache: true).Build().CacheTime().RunString();
                    return JsonData.FirstOrDefault().ToModel<List<DeveloperRoot>>();
                }, ex =>
                {
                    action?.Invoke(ex);
                    return null;
                });
            });
        }

        public static string GetVersion(Action<Exception> action = null)
        {
            return RetryException.DoRetry(() =>
            {
                return XPlusEx.XTry(() =>
                {
                    return HttpMultiClient.HttpMulti.AddNode(BaseURL + VersionURL).Build().CacheTime().RunString().FirstOrDefault();
                }, ex =>
                {
                    action?.Invoke(ex);
                    return null;
                });
            });
        }
    }
}
