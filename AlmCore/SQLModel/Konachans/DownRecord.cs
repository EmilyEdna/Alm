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
        /// <summary>
        /// 进度
        /// </summary>
        [SugarColumn(IsNullable=true)]
        public double? Progress { get; set; }
        /// <summary>
        /// 当前下载位置
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public long? CurrentStream { get; set; }
        /// <summary>
        /// 总长
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public long? TotalStream { get; set; }
    }
}
