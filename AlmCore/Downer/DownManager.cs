using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlmCore.Downer
{
    public class DownManager
    {
        private long fromIndex = 0;//开始下载的位置
        private bool isRun = false;//是否正在进行
        private DownInfo dlInfo;

        private List<DownService> dls = new List<DownService>();

        public event Action OnStart;
        public event Action OnStop;
        public event Action<long, long> OnDownload;
        public event Action OnFinsh;

        public DownManager(DownInfo dlInfo)
        {
            this.dlInfo = dlInfo;
        }
        public void Stop()
        {
            this.isRun = false;
            dls.ForEach(dl => dl.Stop());
            OnStopHandler();
        }

        public void Start()
        {
            this.dlInfo.IsReStart = false;
            WorkStart();
        }
        public void ReStart()
        {
            this.dlInfo.IsReStart = true;
            WorkStart();
        }

        private void WorkStart()
        {
            try
            {
                if (dlInfo.IsReStart)
                {
                    this.Stop();
                }

                while (dls.Where(dl => !dl.IsStopped).Count() > 0)
                {
                    if (dlInfo.IsReStart) Thread.Sleep(100);
                    else return;
                }

                this.isRun = true;
                OnStartHandler();
                //首次任务或者不支持断点续传的进入
                if (dlInfo.IsNewTask || (!dlInfo.IsNewTask && !dlInfo.IsSupportMultiThreading))
                {
                    //第一次请求获取一小块数据，根据返回的情况判断是否支持断点续传
                    using (var rsp = HttpCore.RangeDown(dlInfo.DownloadUrlList[0], 0, 0))
                    {

                        //获取文件名，如果包含附件名称则取下附件，否则从url获取名称
                        var Disposition = rsp.Headers["Content-Disposition"];
                        if (Disposition != null) dlInfo.FileName = Disposition.Split('=')[1];
                        else dlInfo.FileName = Path.GetFileName(rsp.ResponseUri.AbsolutePath);

                        //默认给流总数
                        dlInfo.Count = rsp.ContentLength;
                        //尝试获取 Content-Range 头部，不为空说明支持断点续传
                        var contentRange = rsp.Headers["Content-Range"];
                        if (contentRange != null)
                        {
                            //支持断点续传的话，就取range 这里的总数
                            dlInfo.Count = long.Parse(rsp.Headers["Content-Range"]?.Split('/')?[1]);
                            dlInfo.IsSupportMultiThreading = true;

                            //生成一个临时文件名
                            var tempFileName = Convert.ToBase64String(Encoding.UTF8.GetBytes(dlInfo.FileName)).ToUpper();
                            tempFileName = tempFileName.Length > 32 ? tempFileName.Substring(0, 32) : tempFileName;
                            dlInfo.TempFileName = tempFileName + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            ///创建线程信息
                            ///
                            GetTaskInfo(dlInfo);

                        }
                        else
                        {
                            //不支持断点续传则一开始就直接读完整流
                            Save(GetRealFileName(dlInfo), rsp.GetResponseStream());
                            OnFineshHandler();
                        }
                    }
                    dlInfo.IsNewTask = false;
                }
                //如果支持断点续传采用这个
                if (dlInfo.IsSupportMultiThreading)
                {
                    StartTask(dlInfo);

                    //等待合并
                    while (this.dls.Where(td => !td.IsFinish).Count() > 0 && this.isRun)
                    {
                        Thread.Sleep(100);
                    }
                    if (dls.Where(td => !td.IsFinish).Count() == 0)
                    {

                        CombineFiles(dlInfo);
                        OnFineshHandler();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CombineFiles(DownInfo dlInfo)
        {
            string realFilePath = GetRealFileName(dlInfo);

            //合并数据
            byte[] buffer = new Byte[2048];
            int length = 0;
            using (var fileStream = File.Open(realFilePath, FileMode.CreateNew))
            {
                for (int i = 0; i < dlInfo.TaskInfoList.Count; i++)
                {
                    var tempFile = dlInfo.TaskInfoList[i].FilePath;
                    using (var tempStream = File.Open(tempFile, FileMode.Open))
                    {
                        while ((length = tempStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fileStream.Write(buffer, 0, length);
                        }
                        tempStream.Flush();
                    }
                    //File.Delete(tempFile);
                }
            }
        }

        private static string GetRealFileName(DownInfo dlInfo)
        {
            //创建正式文件名，如果已存在则加数字序号创建，避免覆盖
            var fileIndex = 0;
            var realFilePath = Path.Combine(dlInfo.SaveDir, dlInfo.FileName);
            while (File.Exists(realFilePath))
            {
                realFilePath = Path.Combine(dlInfo.SaveDir, string.Format("{0}_{1}", fileIndex++, dlInfo.FileName));
            }

            return realFilePath;
        }

        private void StartTask(DownInfo dlInfo)
        {
            dls = new List<DownService>();
            if (dlInfo.TaskInfoList != null)
            {
                foreach (var item in dlInfo.TaskInfoList)
                {
                    var dl = new DownService();
                    dl.OnDownload += OnDownloadHandler;
                    dls.Add(dl);
                    dl.Start(item, dlInfo.IsReStart);
                }
            }
        }

        private void GetTaskInfo(DownInfo dlInfo)
        {
            var pieceSize = (dlInfo.Count) / dlInfo.TaskCount;
            dlInfo.TaskInfoList = new List<TaskInfo>();
            var rand = new Random();
            var urlIndex = 0;
            for (int i = 0; i <= dlInfo.TaskCount + 1; i++)
            {
                var from = (i * pieceSize);

                if (from >= dlInfo.Count) break;
                var to = from + pieceSize;
                if (to >= dlInfo.Count) to = dlInfo.Count;

                dlInfo.TaskInfoList.Add(
                    new TaskInfo
                    {
                        DownloadUrl = dlInfo.DownloadUrlList[urlIndex++],
                        FilePath = Path.Combine(dlInfo.SaveDir, dlInfo.TempFileName + i + ".temp"),
                        FromIndex = from,
                        ToIndex = to
                    });
                if (urlIndex >= dlInfo.DownloadUrlList.Count) urlIndex = 0;
            }
        }

        /// <summary>
        /// 保存内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="stream"></param>
        private void Save(string filePath, Stream stream)
        {
            using (var writer = File.Open(filePath, FileMode.Append))
            {
                using (stream)
                {
                    var repeatTimes = 0;
                    byte[] buffer = new byte[1024];
                    var length = 0;
                    while ((length = stream.Read(buffer, 0, buffer.Length)) > 0 && this.isRun)
                    {
                        writer.Write(buffer, 0, length);
                        this.fromIndex += length;
                        if (repeatTimes % 5 == 0)
                        {
                            writer.Flush();//一定大小就刷一次缓冲区
                            OnDownloadHandler();
                        }
                        repeatTimes++;
                    }
                    writer.Flush();
                    OnDownloadHandler();
                }
            }
        }


        private void OnStartHandler()
        {
            new Action(() =>
            {
                this.OnStart?.Invoke();
            }).Invoke();
        }
        private void OnStopHandler()
        {
            new Action(() =>
            {
                this.OnStop?.Invoke();
            }).Invoke();
        }
        private void OnFineshHandler()
        {
            new Action(() =>
            {
                for (int i = 0; i < dlInfo.TaskInfoList.Count; i++)
                {
                    var tempFile = dlInfo.TaskInfoList[i].FilePath;
                    File.Delete(tempFile);
                }
                this.OnFinsh?.Invoke();
            }).Invoke();
        }
        private void OnDownloadHandler()
        {
            new Action(() =>
            {
                long current = GetDownloadLength();
                this.OnDownload?.Invoke(current, dlInfo.Count);
            }).Invoke();
        }

        public long GetDownloadLength()
        {
            if (dlInfo.IsSupportMultiThreading) return dls.Sum(dl => dl.GetDownloadedCount());
            else return this.fromIndex;
        }
    }
}
