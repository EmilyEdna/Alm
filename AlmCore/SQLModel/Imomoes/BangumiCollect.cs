using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore.SQLModel.Imomoes
{
    /// <summary>
    /// 番剧收藏
    /// </summary>
    [SugarTable("BangumiCollect")]
    public class BangumiCollect: ISQLModel
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 番剧名称
        /// </summary>
        public string BangumiName { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string Conver { get; set; }
        /// <summary>
        /// 详情页面
        /// </summary>
        public string ShortURL { get; set; }
    }
}
