using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore.Downer
{
    public class DownInfo
    {
        /// <summary>
        /// 子线程数量
        /// </summary>
        public int TaskCount { get; set; } = 1;
        /// <summary>
        /// 缓存名，临时保存的文件名
        /// </summary>
        public string TempFileName { get; set; }
        /// <summary>
        /// 是否是新任务，如果不是新任务则通过配置去分配线程
        /// 一开始要设为true,在初始化完成后会被设为true，此时可以对这个 DownloadInfo 进行序列化后保存，进而实现退出程序加载配置继续下载。
        /// </summary>
        public bool IsNewTask { get; set; } = true;
        /// <summary>
        /// 是否重新下载
        /// </summary>
        public bool IsReStart { get; set; } = false;
        /// <summary>
        /// 任务总大小
        /// </summary>
        public long Count { get; set; }
        /// <summary>
        /// 保存的目录
        /// </summary>
        public string SaveDir { get; set; }
        /// <summary>
        /// 文件民初
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 下载地址，
        /// 这里是列表形式，如果同一个文件有不同来源则可以通过不同来源取数据
        /// 来源的有消息需另外判断
        /// </summary>
        public List<string> DownloadUrlList { get; set; }
        /// <summary>
        /// 是否支持断点续传
        /// 在任务开始后，如果需要暂停，应先通过这个判断是否支持
        /// 默认设为false
        /// </summary>
        public bool IsSupportMultiThreading { get; set; } = false;
        /// <summary>
        /// 线程任务列表
        /// </summary>
        public List<TaskInfo> TaskInfoList { get; set; }
    }
}
