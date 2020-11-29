using AlmCore.ThreadDowner.FileInfos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AlmCore.ThreadDowner
{
    public class ThreadMainCore
    {
        public static ThreadMainCore Instance => new Lazy<ThreadMainCore>().Value;

        /// <summary>
        /// 主线程下载
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public FileInfoDowner MainHttp(string Url)
        {
            FileInfoDowner fileInfo = new FileInfoDowner
            {
                SaveDir = Extension.CreateDir(Extension.SavrDir)
            };
            var rsp = HttpCore.RangeDown(Url, 0, 0);
            //获取文件名，如果包含附件名称则取下附件，否则从url获取名称
            var Disposition = rsp.Headers["Content-Disposition"];
            if (Disposition != null) fileInfo.FileName = Disposition.Split('=')[1];
            else fileInfo.FileName = Path.GetFileName(rsp.ResponseUri.AbsolutePath);
            //默认给流总数
            fileInfo.Count = rsp.ContentLength;
            //尝试获取 Content-Range 头部，不为空说明支持断点续传
            var contentRange = rsp.Headers["Content-Range"];
            if (contentRange != null)
            {
                var tempFileName = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileInfo.FileName)).ToUpper();
                tempFileName = tempFileName.Length > 32 ? tempFileName.Substring(0, 32) : tempFileName;
                fileInfo.TempFileName = tempFileName + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                fileInfo.AllowTask = true;
                fileInfo.Url = Url;
                fileInfo.Count = long.Parse(rsp.Headers["Content-Range"]?.Split('/')?[1]);
            }
            else
            {
                fileInfo.AllowTask = false;
                var FullDir = fileInfo.SaveDir + "\\" + fileInfo.FileName;
                Extension.CreateFile(FullDir);
                SaveFile(FullDir, rsp.GetResponseStream());
            }
            return fileInfo;
        }
        /// <summary>
        /// 不支持多线程则单线程一次性下载并保存文件
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="rsp"></param>
        private void SaveFile(string FileName, Stream stream)
        {
            var fromIndex = 0;
            using var writer = File.Open(FileName, FileMode.Append);
            var repeatTimes = 0;
            byte[] buffer = new byte[1024];
            var length = 0;
            while ((length = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                writer.Write(buffer, 0, length);
                fromIndex += length;
                if (repeatTimes % 5 == 0)
                {
                    writer.Flush();//一定大小就刷一次缓冲区
                }
                repeatTimes++;
            }
            writer.Flush();
            stream.Flush();
            stream.Close();
        }
    }
}
