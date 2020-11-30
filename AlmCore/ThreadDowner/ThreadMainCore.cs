using AlmCore.ThreadDowner.FileInfos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlmCore.ThreadDowner
{
    public class ThreadMainCore
    {
        public static ThreadMainCore Instance => new Lazy<ThreadMainCore>().Value;

        /// <summary>
        /// 多线程下载
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public void MainHttp(string Url, string Name)
        {
            FileInfoDowner fileInfo = new FileInfoDowner
            {
                SaveDir = Extension.CreateDir(Extension.SavrDir),
                FileName = Name
            };
            var rsp = HttpCore.RangeDown(Url, 0, 0);

            //尝试获取 Content-Range 头部，不为空说明支持断点续传
            var contentRange = rsp.Headers["Content-Range"];
            if (contentRange != null)
            {
                var tempFileName = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileInfo.FileName)).ToUpper();
                tempFileName = tempFileName.Length > 32 ? tempFileName.Substring(0, 32) : tempFileName;
                fileInfo.Count = long.Parse(rsp.Headers["Content-Range"]?.Split('/')?[1]);
            }
            new Thread(() => Save(fileInfo.FullPath, rsp.GetResponseStream())).Start();
        }
        private void Save(string FileName, Stream st)
        {
            byte[] bytes = new byte[1024];
            if (File.Exists(FileName)) File.Delete(FileName);
            using var fs = File.Open(FileName, FileMode.CreateNew);
            int length;
            while ((length = st.Read(bytes, 0, bytes.Length)) > 0)
            {
                fs.Write(bytes, 0, length);
            }
            fs.Close();
        }

    }
}
