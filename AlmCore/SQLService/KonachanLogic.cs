using AlmCore.SQLModel.Konachans;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore.SQLService
{
    public class KonachanLogic
    {
        public static KonachanLogic Logic => new KonachanLogic();
        /// <summary>
        /// 获取用户自定义标签
        /// </summary>
        /// <returns></returns>
        public List<UserTags> GetUserTags()
        {
            return SQLContext.Lite.Queryable<UserTags>().OrderBy(t => t.AddTime, OrderByType.Desc).ToList();
        }
    }
}
