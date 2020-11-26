using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore.Downer
{
    public class TaskInfo
    {
        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownloadUrl { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 分片起点
        /// </summary>
        public long FromIndex { get; set; }
        /// <summary>
        /// 分片终点
        /// </summary>
        public long ToIndex { get; set; }
        /// <summary>
        /// 分片的总大小
        /// </summary>
        public long Count => ToIndex - FromIndex + 1;
    }
}
