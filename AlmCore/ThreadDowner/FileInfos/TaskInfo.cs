using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AlmCore.ThreadDowner.FileInfos
{
    public class TaskInfo
    {
        public int Id { get; set; }
        /// <summary>
        /// 分片大小
        /// </summary>
        public long PartialSize { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 分片名称 合并需要知道顺序
        /// </summary>
        public string PartialName { get; set; }
        /// <summary>
        /// 开始位置
        /// </summary>
        public long FromIndex { get; set; }
        /// <summary>
        /// 目标位置
        /// </summary>
        public long ToIndex { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsComplate { get; set; } = false;
        /// <summary>
        /// 真实名字
        /// </summary>
        public string RealFileName { get;  set; }
    }
}
