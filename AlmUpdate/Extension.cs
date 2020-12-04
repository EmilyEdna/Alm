using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Text;

namespace AlmUpdate
{
    public  class Extension
    {
        private const string ReleaseURL = "https://raw.githubusercontent.com/EmilyEdna/Alm/master/Configs/UpdateUrl.txt";
        public event Action<long, long> Progress;
        private int Long = 0;
        /// <summary>
        /// 获取升级地址
        /// </summary>
        /// <returns></returns>
        public  void GetFilesList()
        {
            WebClient client = new WebClient();
            GetZipFile(client.DownloadString(ReleaseURL).Replace("\n", ""));
        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="URL"></param>
        private  void GetZipFile(string URL)
        {
            HttpWebRequest request = null;
            if (URL.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((sender, certificate, chain, errors) => true);
                request = WebRequest.Create(URL) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version11;
            }
            else
            {
                request = WebRequest.Create(URL) as HttpWebRequest;
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var Size = response.ContentLength;
            var st = response.GetResponseStream();
            byte[] bytes = new byte[1024];
            using var fs = File.Open(AppDomain.CurrentDomain.BaseDirectory+ "AlmUpdate.zip", FileMode.CreateNew);
            int length = 0;
            while ((length = st.Read(bytes, 0, bytes.Length)) > 0)
            {
                Long += length;
                fs.Write(bytes, 0, length);
                Progress?.Invoke(Long, Size);
            }
            fs.Close();
            ExtractZip();
        }

        /// <summary>
        /// 自解压文件
        /// </summary>
        private  void ExtractZip()
        {
            string extractPath = AppDomain.CurrentDomain.BaseDirectory;
            string zipPath = extractPath + "AlmUpdate.zip";
            ZipFile.ExtractToDirectory(zipPath, extractPath);
            File.Delete(zipPath);
        }
    }

}
