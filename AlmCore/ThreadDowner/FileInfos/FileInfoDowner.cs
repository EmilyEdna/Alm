using System;
using System.Collections.Generic;
using System.IO;
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
        /// 保存路径
        /// </summary>
        public string SaveDir { get; set; }
        /// <summary>
        /// 下载地址
        /// </summary>
        public string Url { get;  set; }
        /// <summary>
        /// 流大小
        /// </summary>
        public long Count { get; set; }
        /// <summary>
        /// 完整路径
        /// </summary>
        public string FullPath => Path.Combine(SaveDir, FileName);
    }
}
