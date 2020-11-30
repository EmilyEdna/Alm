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

        public event Action<long, long> Progress;
        private int Long = 0;
        private static bool Flag = false;
        /// <summary>
        /// 多线程下载
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public ThreadMainCore MainHttp(string Url)
        {
            FileInfoDowner fileInfo = new FileInfoDowner
            {
                SaveDir = Extension.CreateDir(Extension.SavrDir),
                Url = Url,
            };
            var rsp = HttpCore.RangeDown(Url, 0, 0);

            //获取文件名，如果包含附件名称则取下附件，否则从url获取名称
            var Disposition = rsp.Headers["Content-Disposition"];
            if (Disposition != null) fileInfo.FileName = Disposition.Split('=')[1];
            else fileInfo.FileName = Path.GetFileName(rsp.ResponseUri.AbsolutePath);

            //尝试获取 Content-Range 头部，不为空说明支持断点续传
            var contentRange = rsp.Headers["Content-Range"];
            if (contentRange != null)
            {
                var tempFileName = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileInfo.FileName)).ToUpper();
                tempFileName = tempFileName.Length > 32 ? tempFileName.Substring(0, 32) : tempFileName;
                fileInfo.Count = long.Parse(rsp.Headers["Content-Range"]?.Split('/')?[1]);
                Task.Factory.StartNew(() => Save(fileInfo));
            }
            else
            {
                Task.Factory.StartNew(() => Save(fileInfo, rsp.GetResponseStream()));
            }
            return this;
        }

        private void Save(FileInfoDowner fileInfo, Stream st)
        {
            byte[] bytes = new byte[1024];
            if (File.Exists(fileInfo.FullPath)) File.Delete(fileInfo.FullPath);
            using var fs = File.Open(fileInfo.FullPath, FileMode.CreateNew);
            int length = 0;
            while ((length = st.Read(bytes, 0, bytes.Length)) > 0)
            {
                Long += length;
                fs.Write(bytes, 0, length);
                Progress.Invoke(Long, fileInfo.Count);
            }
            fs.Close();
        }

        private void Save(FileInfoDowner fileInfo)
        {
            Stream st = HttpCore.RangeDown(fileInfo.Url, 0, fileInfo.Count).GetResponseStream();
            byte[] bytes = new byte[1024];
            if (File.Exists(fileInfo.FullPath)) File.Delete(fileInfo.FullPath);
            using var fs = File.Open(fileInfo.FullPath, FileMode.CreateNew);
            int length = 0;
            while ((length = st.Read(bytes, 0, bytes.Length)) > 0)
            {
                if (Flag)
                    break;
                Long += length;
                fs.Write(bytes, 0, length);
                Progress?.Invoke(Long, fileInfo.Count);
            }
            fs.Close();
        }
    }
}
