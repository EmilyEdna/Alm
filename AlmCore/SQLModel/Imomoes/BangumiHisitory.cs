using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore.SQLModel.Imomoes
{
    /// <summary>
    /// 播放历史
    /// </summary>
    [SugarTable("BangumiHisitory")]
    public class BangumiHisitory : ISQLModel
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 视频地址
        /// </summary>
        [SugarColumn(Length = 500)]
        public string BangumiURL { get; set; }
        /// <summary>
        /// 番剧名称
        /// </summary>
        public string BangumiName { get; set; }
        /// <summary>
        /// 集数
        /// </summary>
        public string Collection { get; set; }
        /// <summary>
        /// 播放进度
        /// </summary>
        [SugarColumn(Length = 200)]
        public string PlayProgress { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public double SpanSeconds { get; set; }
        /// <summary>
        /// 观看时间
        /// </summary>
        [SugarColumn(ColumnDataType = "DateTime")]
        public DateTime PlayTime { get; set; }
    }
}
