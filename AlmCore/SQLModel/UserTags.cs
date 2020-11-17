using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore.SQLModel
{
    [SugarTable("UserTags")]
    public class UserTags
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        public int TagsId { get; set; }
        /// <summary>
        /// 自定义名称
        /// </summary>
        public string DiyName { get; set; }
        [SugarColumn(ColumnDataType = "DateTime")]
        public DateTime AddTime { get; set; }
    }
}
