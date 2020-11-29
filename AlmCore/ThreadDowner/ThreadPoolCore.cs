using AlmCore.ThreadDowner.FileInfos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using XExten.XPlus;
using XExten.XCore;
using System.Web;

namespace AlmCore.ThreadDowner
{
    public class ThreadPoolCore
    {
        public static ThreadPoolCore Instance => new Lazy<ThreadPoolCore>().Value;
        List<FileHelper> LF = new List<FileHelper>();
        public static bool Run = true;
        public void CreateTaskInfo(FileInfoDowner fileInfo)
        {
            var PartialSize = fileInfo.Count / fileInfo.TaskCount;
            List<TaskInfo> taskInfos = new List<TaskInfo>();
            for (int i = 0; i <= fileInfo.TaskCount + 1; i++)
            {
                var From = i * PartialSize;
                var To = From + PartialSize;
                if (From >= fileInfo.Count) break;
                if (To >= fileInfo.Count) To = fileInfo.Count;
                taskInfos.Add(new TaskInfo
                {
                    Id = i,
                    Url = fileInfo.Url,
                    FromIndex = From,
                    ToIndex = To,
                    PartialName = Path.Combine(fileInfo.SaveDir, fileInfo.TempFileName + "_" + i + ".temp"),
                    PartialSize = PartialSize,
                    RealFileName = fileInfo.FileName
                });
            }
            ClearPutLogMes(taskInfos);
            ThreadPoolDown(taskInfos);
        }
        private void ThreadPoolDown(List<TaskInfo> infos)
        {
            infos.Where(t => t.Id <= 2).ToList().ForEach(item =>
            {
                Task.Factory.StartNew(() => ThreadPoolDown(item));
            });
            infos.Where(t => t.Id > 2 && t.Id <= 4).ToList().ForEach(item =>
            {
                Task.Factory.StartNew(() => ThreadPoolDown(item));
            });
            infos.Where(t => t.Id > 4).ToList().ForEach(item =>
            {
                Task.Factory.StartNew(() => ThreadPoolDown(item));
            });
        }
        private void ThreadPoolDown(TaskInfo Info)
        {
            File.Create(Info.PartialName).DisposeAsync();
            var rsp = HttpCore.RangeDown(Info.Url, Info.FromIndex, Info.ToIndex);
            Save(Info.PartialName, rsp.GetResponseStream());
        }
        private void Save(string filePath, Stream stream)
        {
            try
            {
                using var writer = File.Open(filePath, FileMode.Append);
                byte[] buffer = new byte[1024];
                var length = 0;
                while ((length = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    writer.Write(buffer, 0, length);
                }
                stream.Flush();
                stream.Close();
            }
            catch (Exception)
            {
                Save(filePath, stream);
            }
        }

        private void ReadOnMemory(List<TaskInfo> Info)
        {
            var FullName = Extension.SavrDir + "\\" + HttpUtility.UrlDecode(Info.FirstOrDefault().RealFileName);
            foreach (var item in Info.Where(t => t.IsComplate == false).OrderBy(t => t.Id))
            {
                if (File.Exists(item.PartialName))
                {
                    XPlusEx.XTry(() =>
                    {
                        Stream stream = File.Open(item.PartialName, FileMode.Open);
                        var leng = (item.ToIndex - item.FromIndex) + 1;
                        if (Info.Max(t => t.Id) == item.Id)
                            leng -= 1;
                        if (stream.Length != 0 && stream.Length == leng)
                        {
                            item.IsComplate = true;
                            LF.Add(new FileHelper
                            {
                                Id = item.Id,
                                IsComplate = true,
                                Sbytes = Extension.StreamToBytes(stream),
                                PartialName = item.PartialName
                            });
                        }
                    }, ex =>
                    {
                        Thread.Sleep(3000);
                        ReadOnMemory(Info);
                    });
                }
            }

            if (LF.Count != 0 && Info.Count == LF.Count)
            {
                Dictionary<int, byte[]> Lb = new Dictionary<int, byte[]>();
                LF.Where(t => t.IsComplate == true).OrderBy(t => t.Id).ToEachs(item =>
                {
                    Lb.Add(item.Id, item.Sbytes);

                    item.IsComplate = false;
                });
                if (Lb.Count == 5)
                {
                    byte[] Bytes = new byte[Lb[0].Length + Lb[1].Length + Lb[2].Length + Lb[3].Length + Lb[4].Length];
                    Lb[0].CopyTo(Bytes, 0);
                    Lb[1].CopyTo(Bytes, Lb[0].Length);
                    Lb[2].CopyTo(Bytes, Lb[1].Length);
                    Lb[3].CopyTo(Bytes, Lb[2].Length);
                    Lb[4].CopyTo(Bytes, Lb[3].Length);
                    FileStream fs = File.Open(FullName, FileMode.CreateNew);
                    fs.Read(Bytes, 0, Bytes.Length);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(Bytes);
                    bw.Close();
                    fs.Close();
                }
                if (LF.Where(t => t.IsComplate == false).Count() == 5)
                {
                    Run = false;
                    //LF.ForEach(x =>
                    //{
                    //    File.Delete(x.PartialName);
                    //});
                    LF.Clear();
                }
            }
        }

        /// <summary>
        /// 线程循环监听
        /// </summary>
        private void ClearPutLogMes(List<TaskInfo> Info)
        {
            var thread = new Thread(() =>
             {
                 while (Run)
                 {
                     new Action(() =>
                     {
                         ReadOnMemory(Info);
                     }).Invoke();
                 }
             });
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
