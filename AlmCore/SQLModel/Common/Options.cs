using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore.SQLModel.Common
{
    /// <summary>
    /// 通用配置
    /// </summary>
    [SugarTable("Options")]
    public class Options : ISQLModel
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        /// <summary>
        /// 查询页数
        /// </summary>
        public int OptionPage { get; set; }
        /// <summary>
        /// 解析地址一
        /// </summary>
        public string AddrOne { get; set; }
        /// <summary>
        /// 解析地址二
        /// </summary>
        public string AddrTwo { get; set; }
        /// <summary>
        /// 默认解析地址
        /// </summary>
        public string DefaultAddr { get; set; }
    }
}
