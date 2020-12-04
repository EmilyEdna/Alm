using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using XExten.XPlus;

namespace AlmCore
{
    public class Extension
    {
        public static string ApplicationRoute = AppDomain.CurrentDomain.BaseDirectory;
        public static string Connection = ApplicationRoute + "Alm.SqlLite";
        public static string InitDataBase = ApplicationRoute + "Alm.cof";
        public static string SavrDir = ApplicationRoute + "Save";

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
    }
}
