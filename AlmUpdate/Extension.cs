using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace AlmUpdate
{
    public static class Extension
    {
        private const string ReleaseURL = "";

        public static void GetFilesList()
        {
            WebClient client = new WebClient();
        }

        /// <summary>
        /// 自解压文件
        /// </summary>
        public static void ExtractZip()
        {
            string extractPath = AppDomain.CurrentDomain.BaseDirectory;
            string zipPath = extractPath + "AlmUpdate.zip";
            ZipFile.ExtractToDirectory(zipPath, extractPath);
            File.Delete(zipPath);
        }
    }

}
