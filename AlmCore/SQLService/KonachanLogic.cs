using AlmCore.Scrapy.KonachanModel;
using AlmCore.SQLModel.Konachans;
using SqlSugar;
using System;
using XExten.XCore;
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
        /// <summary>
        /// 检查是否收藏过
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool HasCollect(long Id)
        {
            return SQLContext.Lite.Queryable<KonaCollect>().Where(t => t.Id == Id).First() == null;
        }
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="Elements"></param>
        public void AddCollect(ImageElements Elements)
        {
            KonaCollect collect = Elements.ToAutoMapper<KonaCollect>();
            SQLContext.Lite.Insertable(collect).ExecuteCommand();
        }
        /// <summary>
        /// 删除收藏
        /// </summary>
        /// <param name="Id"></param>
        public void RemoveCollect(long Id)
        {
            SQLContext.Lite.Deleteable<KonaCollect>(t => t.Id == Id).ExecuteCommand();
        }
    }
}
