using NeteaseCloudMusicApi;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XExten.XPlus;

namespace AlmCore.Scrapy
{
    public class Music
    {
        private static readonly ConcurrentDictionary<string, CloudMusicApi> Pairs = new ConcurrentDictionary<string, CloudMusicApi>();

        public static void InitApi()
        {
            if (!Pairs.ContainsKey(nameof(CloudMusicApi)))
            {
                Pairs.TryAdd(nameof(CloudMusicApi), new CloudMusicApi());
            }
        }

        public static void Clear()
        {
            Pairs.Clear();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public static async Task<JObject> Login(string Account = "16716582402", string Pwd = "dev123456")
        {
            Dictionary<string, string> requests = new Dictionary<string, string>();
            bool IsPhone = Regex.Match(Account, "^[0-9]+$").Success;
            requests[IsPhone ? "phone" : "email"] = Account;
            requests["password"] = Pwd;
            CloudMusicApi api = Pairs[nameof(CloudMusicApi)];
            (bool IsOk, JObject Data) = await api.RequestAsync(IsPhone ? CloudMusicApiProviders.LoginCellphone : CloudMusicApiProviders.Login, requests);
            return Data;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public static async Task<JObject> LoginOut()
        {
            CloudMusicApi api = Pairs[nameof(CloudMusicApi)];
            (bool IsOk, JObject Data) = await api.RequestAsync(CloudMusicApiProviders.Logout, CloudMusicApi.EmptyQueries);
            return Data;
        }

        /// <summary>
        /// 获取用户歌单
        /// </summary>
        /// <param name="Id">用户Id</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static async Task<JObject> GetUserInfo(string Id, int PageIndex = 0, int PageSize = 30)
        {
            Dictionary<string, string> requests = new Dictionary<string, string>
            {
                ["uid"] = Id,
                ["limit"] = PageSize.ToString(),
                ["offset"] = PageIndex.ToString()
            };
            CloudMusicApi api = Pairs[nameof(CloudMusicApi)];
            (bool IsOk, JObject Data) = await api.RequestAsync(CloudMusicApiProviders.UserPlaylist, requests);
            return Data;
        }

        /// <summary>
        /// 用户播放记录
        /// </summary>
        /// <param name="Id">用户Id</param>
        /// <param name="Type">1：本周，0：所有日期</param>
        /// <returns></returns>
        public static async Task<JObject> PlayRecord(string Id, int Type = 1) 
        {
            Dictionary<string, string> requests = new Dictionary<string, string>
            {
                ["uid"] = Id,
                ["type"] = Type.ToString(),
            };
            CloudMusicApi api = Pairs[nameof(CloudMusicApi)];
            (bool IsOk, JObject Data) = await api.RequestAsync(CloudMusicApiProviders.UserRecord, requests);
            return Data;
        }

        /// <summary>
        /// 获取音乐 url
        /// </summary>
        /// <param name="Id">歌曲Id</param>
        /// <param name="Hz">码率,默认设置了 999000 即最大码率,如果要 320k 则可设置为 320000,其他类推</param>
        /// <returns></returns>
        public static async Task<JObject> GetMusicURL(string Id, long Hz = 999000) {
            Dictionary<string, string> requests = new Dictionary<string, string>
            {
                ["id"] = Id,
                ["br"] = Hz.ToString(),
            };
            CloudMusicApi api = Pairs[nameof(CloudMusicApi)];
            (bool IsOk, JObject Data) = await api.RequestAsync(CloudMusicApiProviders.SongUrl, requests);
            return Data;
        }

        /// <summary>
        /// 日推
        /// </summary>
        /// <returns></returns>
        public static async Task<JObject> DaySongs()
        {
            CloudMusicApi api = Pairs[nameof(CloudMusicApi)];
            (bool IsOk, JObject Data) = await api.RequestAsync(CloudMusicApiProviders.RecommendSongs, CloudMusicApi.EmptyQueries);
            return Data;
        }
    }
}
