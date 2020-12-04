using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlmUpdate
{
    public class Extension
    {
        private const string ReleaseURL = "https://raw.githubusercontent.com/EmilyEdna/Alm/master/Configs/UpdateUrl.txt";
        private string Base = AppDomain.CurrentDomain.BaseDirectory;
        public event Action<long, long> Progress;
        private int Long = 0;
        /// <summary>
        /// 获取升级地址
        /// </summary>
        /// <returns></returns>
        public void GetFilesList()
        {
            WebClient client = new WebClient();
            var URL = client.DownloadString(ReleaseURL).Replace("\n", "");
            GetZipFile(URL);
        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="URL"></param>
        private void GetZipFile(string URL)
        {
            try
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
                File.Delete(Base + "AlmUpdate.zip");
                using var fs = File.Open(Base + "AlmUpdate.zip", FileMode.CreateNew);
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
            catch (Exception)
            {
                var result = MessageBox.Show("已是最新版本!", "通知", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
            }

        }
        /// <summary>
        /// 自解压文件
        /// </summary>
        private void ExtractZip()
        {
            string zipPath = Base + "AlmUpdate.zip";
            ZipFile.ExtractToDirectory(zipPath, Base);
            OpenAlm();
        }
        /// <summary>
        /// 自动打开主程序
        /// </summary>
        private void OpenAlm()
        {
            Process process = new Process();
            process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "Alm.exe";
            process.StartInfo.CreateNoWindow = true;
            process.Start();//启动
            process.CloseMainWindow();//通过向进程的主窗口发送关闭消息来关闭拥有用户界面的进程
            process.Close();//释放与此组件关联的所有资源
            Environment.Exit(0);
        }
    }
}
