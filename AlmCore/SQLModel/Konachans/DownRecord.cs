using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using XExten.XCore;

namespace AlmCore.SQLModel.Konachans
{
    [SugarTable("DownRecord")]
    public class DownRecord:ISQLModel
    {
        public long Id { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FileURL { get; set; }
        /// <summary>
        /// 下载时间
        /// </summary>
        [SugarColumn(ColumnDataType = "DateTime")]
        public DateTime DownTime { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }
    }
}
