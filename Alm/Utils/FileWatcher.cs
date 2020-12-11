using AlmCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Alm.Utils
{
    public class FileWatcher
    {
        private Hashtable Watcher = null;
        private Queue<string> PathQueue = new Queue<string>();
        private static object Locker = new object();
        private static FileWatcher _Instance;
        public static FileWatcher Instance
        {
            get
            {
                lock (Locker)
                {
                    if (_Instance == null)
                        _Instance = new FileWatcher();
                    return _Instance;
                }
            }
        }
        public void StartWatcherFile()
        {
            if (Watcher == null || Watcher.Count <= 0)
            {
                Watcher = new Hashtable();
                var Paths = Directory.GetLogicalDrives();
                for (int i = 0; i < Paths.Length; i++)
                {
                    if (Directory.Exists(Paths[i]))
                    {
                        FileSystemWatcher watcher = new FileSystemWatcher
                        {
                            NotifyFilter = NotifyFilters.FileName,
                            IncludeSubdirectories = true,// 必须要的，监控子集目录
                            Path = Paths[i],
                            EnableRaisingEvents = true,
                        };
                        watcher.Created += Watcher_Created;
                        watcher.Created -= Watcher_Created;
                        Watcher.Add("Watcher" + i, watcher);
                    }
                }
            }
        }
        public void StopWatchFile()
        {
            if (Watcher != null && Watcher.Count > 0)
            {
                for (int i = 0; i < Watcher.Count; i++)
                {
                    ((FileSystemWatcher)Watcher["Watcher" + i]).Dispose();
                }
                Watcher.Clear();
                Watcher = null;
            }
        }
        public List<string> GetSavePath() {

            List<string> Paths = new List<string>();
            if (PathQueue.Count <= 0) return Paths;
            for (int i = 0; i < PathQueue.Count; i++)
            {
                Paths.Add(PathQueue.Dequeue());
            }
            return Paths;
        }
        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.Contains(Extension.SavrDir))
            {
                return;
            }
            PathQueue.Enqueue(e.FullPath);
        }
    }
}
