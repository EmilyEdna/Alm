using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore.SQLModel
{
    [SugarTable("Tags")]
    public class Tags : ISQLModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName { get; set; }
        /// <summary>
        /// 标签数量
        /// </summary>
        public int Count { get; set; }
    }
}
