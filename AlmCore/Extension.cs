using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using XExten.XPlus;

namespace AlmCore
{
    public class Extension
    {
        private static string ApplicationRoute = AppDomain.CurrentDomain.BaseDirectory;
        public static string Connection = ApplicationRoute + "God.SqlLite";
        public static string InitDataBase = ApplicationRoute + "Config.cof";
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

    }
}
