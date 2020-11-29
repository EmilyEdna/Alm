using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore.ThreadDowner.FileInfos
{
    public class FileInfoDowner
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 总大小流
        /// </summary>
        public long Count { get; set; }
        /// <summary>
        /// 是否支持多线程下载
        /// </summary>
        public bool AllowTask { get; set; }
        /// <summary>
        /// 保存路径
        /// </summary>
        public string SaveDir { get; set; }
        /// <summary>
        /// 线程数量
        /// </summary>
        public int TaskCount { get; set; } = 4;
        /// <summary>
        /// 下载地址
        /// </summary>
        public string Url { get;  set; }
        /// <summary>
        /// 临时文件
        /// </summary>
        public string TempFileName { get;  set; }
    }
}
