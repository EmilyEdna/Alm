using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore.SQLModel.Konachans
{
    /// <summary>
    /// 用户标签表
    /// </summary>
    [SugarTable("UserTags")]
    public class UserTags:ISQLModel
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        /// <summary>
        /// 标签值
        /// </summary>
        public string DiyValue { get; set; }
        /// <summary>
        /// 自定义名称
        /// </summary>
        public string DiyName { get; set; }
        [SugarColumn(ColumnDataType = "DateTime")]
        public DateTime AddTime { get; set; }
    }
}
