using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore.SQLModel
{
    [SugarTable("Collect")]
    public class Collect : ISQLModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 预览地址
        /// </summary>
        public string PreviewURL { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        public string FileURL { get; set; }
        /// <summary>
        /// 所属标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public string Rating { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
    }
}
