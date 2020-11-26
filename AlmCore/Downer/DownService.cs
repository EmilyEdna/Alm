using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace AlmCore.Downer
{
    public class DownService
    {
        private string DownUrl = string.Empty;
        private string FilePath = string.Empty;
        private long From = 0;
        private long To = 0;
        private long Count = 0;
        private long Size = 524288;//512KB
        private bool IsRun = false;//是否正在进行

        public bool IsFinish { get; private set; } = false;//是否已下载完成
        public bool IsStopped { get; private set; } = true;//是否已停止

        public event Action OnStart;
        public event Action OnDownload;
        public event Action OnFinsh;


        public long GetDownloadedCount() => Count - To + From - 1;
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            IsRun = false;
        }

        public bool Start(TaskInfo Info, bool IsReStart)
        {
            DownUrl = Info.DownloadUrl;
            FilePath = Info.FilePath;
            From = Info.FromIndex;
            To = Info.ToIndex;
            Count = Info.Count;
            IsStopped = false;
            if (File.Exists(FilePath))
            {
                if (IsReStart)
                {
                    File.Delete(FilePath);
                    File.Create(FilePath).Close();
                }
            }
            else
            {
                File.Create(FilePath).Close();
            }
            using (var file = File.Open(FilePath, FileMode.Open))
            {
                From = Info.FromIndex + file.Length;
            }
            if (From >= To)
            {
                OnFineshHandler();
                IsFinish = true;
                IsStopped = true;
                return false;
            }
            OnStartHandler();
            IsRun = true;
            WebResponse rsp;
            while (From < To && IsRun)
            {
                long to;
                if (From + Size >= To - 1)
                    to = To - 1;
                else
                    to = From + Size;
                using (rsp = HttpCore.RangeDown(DownUrl, From, to))
                {
                    Save(FilePath, rsp.GetResponseStream());
                }
            }
            if (!IsRun) IsStopped = true;
            if (From >= To)
            {
                this.IsFinish = true;
                this.IsStopped = true;
                OnFineshHandler();
            }
            return true;
        }

        private void Save(string filePath, Stream stream)
        {
            using (var writer = File.Open(filePath, FileMode.Append))
            {
                using (stream)
                {
                    var repeatTimes = 0;
                    byte[] buffer = new byte[1024];
                    var length = 0;
                    while ((length = stream.Read(buffer, 0, buffer.Length)) > 0 && this.IsRun)
                    {
                        writer.Write(buffer, 0, length);
                        From += length;
                        if (repeatTimes % 5 == 0)
                        {
                            OnDownloadHandler();
                        }
                        repeatTimes++;
                    }
                }
            }
            OnDownloadHandler();
        }

        private void OnDownloadHandler()
        {
            OnDownload?.Invoke();
        }

        private void OnStartHandler()
        {
            OnStart?.Invoke();
        }

        private void OnFineshHandler()
        {
            OnFinsh?.Invoke();
            OnDownload?.Invoke();
        }
    }
}
