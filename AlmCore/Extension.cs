using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using XExten.XPlus;
using AlmCore.SQLService;
using System.Linq;
using XExten.HttpFactory;
using System.Text.RegularExpressions;
using XExten.XCore;
using Newtonsoft.Json.Linq;

namespace AlmCore
{
    public class Extension
    {
        public static string ApplicationRoute = AppDomain.CurrentDomain.BaseDirectory;
        public static string Connection = Path.Combine(CreateDir(Path.Combine(ApplicationRoute, "Config")), "Alm.db");
        public static string InitDataBase = Path.Combine(CreateDir(Path.Combine(ApplicationRoute, "Config")), "Alm.cof");
        public static string SavrDir = ApplicationRoute + "Save";
        public static DirectoryInfo VLCPath = new DirectoryInfo(Path.Combine(ApplicationRoute, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
        private const string Resolv = "https://vip.52jiexi.top/?url=";
        private const string ResolvBackup = "https://cdn.yangju.vip/kc/api.php?url=";
        public static Dictionary<string,string> Headers = new Dictionary<string, string> { { "user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36" } };

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="DirRoute"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string CreateDir(string DirRoute, Action action = null)
        {
            if (!Directory.Exists(DirRoute))
            {
                Directory.CreateDirectory(DirRoute);
                action?.Invoke();
            }
            return DirRoute;
        }
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="FileRoute"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool CreateFile(string FileRoute, Action action = null)
        {
            var Flag = !File.Exists(FileRoute);
            if (Flag)
            {
                File.Create(FileRoute).Dispose();
                action?.Invoke();
            }
            return Flag;
        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="FileRoute"></param>
        /// <returns></returns>
        public static string ReadContent(string FileRoute)
        {
            using StreamReader reader = new StreamReader(FileRoute);
            var res = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
            return res;
        }
        /// <summary>
        /// 写入内容
        /// </summary>
        /// <param name="FileRoute"></param>
        /// <param name="data"></param>
        /// <param name="action"></param>
        public static void WriteContent(string FileRoute, string data, Action action = null)
        {
            using StreamWriter writer = new StreamWriter(FileRoute, false);
            XPlusEx.XTry(() =>
            {
                action?.Invoke();
                writer.Write(data);
            }, ex => throw ex, () =>
            {
                writer.Close();
                writer.Dispose();
            });
        }
        /// <summary>
        /// 流转Bytes
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
        /// <summary>
        /// 流转stream
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
        /// <summary>
        /// 获取视频流
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static string ResolvURL(string Url)
        {
            var ResolvURL = CommonLogic.Logic.GetOptions().Select(t => t.DefaultAddr).FirstOrDefault();
            if (ResolvURL.Equals(Resolv))
            {
                var data = HttpMultiClient.HttpMulti.AddNode(Resolv + Url).Build().RunString();
                var Fitlers = Regex.Match(data.FirstOrDefault(), "=\\s\\S(http).*\"").Value;
                return Regex.Match(Fitlers, "http.*\"").Value.Replace("\"", "");
            }
            if (ResolvURL.Equals(ResolvBackup))
            {
                var data = HttpMultiClient.HttpMulti.AddNode(ResolvBackup + Url).Build().RunString().FirstOrDefault();
                return data.ToModel<JObject>().SelectToken("url").ToString();
            }
            return string.Empty;
        }
    }
}
