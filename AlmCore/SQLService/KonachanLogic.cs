using AlmCore.Scrapy.KonachanModel;
using AlmCore.SQLModel.Konachans;
using SqlSugar;
using System;
using XExten.XCore;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using XExten.Common;

namespace AlmCore.SQLService
{
    public class KonachanLogic
    {
        public static KonachanLogic Logic => new KonachanLogic();
        #region 标签
        /// <summary>
        /// 获取用户自定义标签
        /// </summary>
        /// <returns></returns>
        public List<string> GetUserTags()
        {
          return  SQLContext.Lite.Queryable<UserTags>()
               .OrderBy(t => t.AddTime, OrderByType.Desc)
               .ToList().Select(t => $"{t.DiyValue}[{t.DiyName}]").ToList();
        }
        /// <summary>
        /// 获取用户自定义标签
        /// </summary>
        /// <returns></returns>
        public List<UserTags> GetUserTagsPage(ref int Total, int PageIndex = 0)
        {
            return SQLContext.Lite.Queryable<UserTags>().OrderBy(t => t.AddTime, OrderByType.Desc).ToPageList(PageIndex, 20, ref Total);
        }
        /// <summary>
        /// 添加用户标签
        /// </summary>
        /// <param name="Tags"></param>
        public void AddUserTags(UserTags Tags)
        {
            SQLContext.Lite.Insertable(Tags).ExecuteCommand();
        }
        /// <summary>
        /// 删除用户标签
        /// </summary>
        /// <param name="Id"></param>
        public void RemoveUserTags(int Id)
        {
            SQLContext.Lite.Deleteable<UserTags>(t => t.Id == Id).ExecuteCommand();
        }
        /// <summary>
        /// 获取标签
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="Total"></param>
        /// <returns></returns>
        public List<Tags> GetTags(ref int Total, int PageIndex = 0)
        {
            return SQLContext.Lite.Queryable<Tags>().ToPageList(PageIndex, 20, ref Total);
        }
        #endregion

        #region 收藏
        /// <summary>
        /// 查询收藏
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public PageResult<KonaCollect> GetCollect(DateTime? AddTime, int PageIndex)
        {
            return SQLContext.Lite.Queryable<KonaCollect>()
                .WhereIF(AddTime.HasValue, t => t.Time <= AddTime.Value).ToList()
                .ToPage(PageIndex, 20);
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
        #endregion

        #region 下载
        /// <summary>
        /// 添加下载
        /// </summary>
        /// <param name="Record"></param>
        public void AddDownRecord(DownRecord Record)
        {
            SQLContext.Lite.Insertable(Record).ExecuteCommand();
        }
        /// <summary>
        /// 获取下载列表
        /// </summary>
        /// <param name="DownTime"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public PageResult<DownRecord> GetDownRecord(DateTime? DownTime = null, int PageIndex = 1)
        {
            return SQLContext.Lite.Queryable<DownRecord>()
                    .WhereIF(DownTime.HasValue, t => t.DownTime <= DownTime.Value)
                    .OrderBy(t => t.DownTime, OrderByType.Desc).ToList().ToPage(PageIndex, 20);
        }
        /// <summary>
        /// 更新下载状态
        /// </summary>
        /// <param name="State"></param>
        public void UpdateRecord(DownRecord Record)
        {
            SQLContext.Lite.Updateable<DownRecord>()
                .SetColumns(t => t.Progress == Record.Progress)
                .SetColumns(t => t.CurrentStream == Record.CurrentStream)
                .SetColumns(t => t.TotalStream == Record.TotalStream)
                .Where(t => t.Id == Record.Id).ExecuteCommand();
        }
        /// <summary>
        /// 判断下载记录
        /// </summary>
        /// <returns></returns>
        public bool CheckRecord(long Id)
        {
            return SQLContext.Lite.Queryable<DownRecord>().Where(t => t.Id == Id).First() == null;
        }
        /// <summary>
        /// 删除下载记录
        /// </summary>
        /// <param name="Id"></param>
        public void DeleteRecord(long Id)
        {
            SQLContext.Lite.Deleteable<DownRecord>(t => t.Id == Id).ExecuteCommand();
        }
        /// <summary>
        /// 删除下载记录
        /// </summary>
        public void DeleteRecordAll()
        {
            SQLContext.Lite.Deleteable<DownRecord>().ExecuteCommand();
        }
        #endregion
    }
}
