using AlmCore.SQLModel.Imomoes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XExten.Common;
using XExten.XCore;

namespace AlmCore.SQLService
{
    public class ImomoeLogic
    {
        public static ImomoeLogic Logic => new ImomoeLogic();
        /// <summary>
        /// 添加历史记录
        /// </summary>
        /// <param name="Hisitory"></param>
        /// <returns></returns>
        public int AddHistory(BangumiHisitory Hisitory)
        {
            return SQLContext.Lite.Insertable(Hisitory).ExecuteReturnIdentity();
        }
        /// <summary>
        /// 更新播放历史
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Span"></param>
        /// <returns></returns>
        public Task UpdateHistory(int Id, string Span)
        {
            return SQLContext.Lite.Updateable<BangumiHisitory>()
                 .SetColumns(t => t.PlayProgress == Span)
                 .Where(t => t.Id == Id).ExecuteCommandAsync();
        }
        /// <summary>
        /// 检查播放历史
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Collection"></param>
        /// <returns></returns>
        public bool CheckHistory(string Name, string Collection)
        {
            BangumiHisitory Bangumi = SQLContext.Lite.Queryable<BangumiHisitory>()
                   .Where(t => t.BangumiName.Equals(Name))
                   .Where(t => t.Collection.Equals(Collection)).First();
            return Bangumi == null;
        }
        /// <summary>
        /// 查询历史记录
        /// </summary>
        /// <param name="PlayTime"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public PageResult<BangumiHisitory> GetPlayHistories(DateTime? PlayTime = null, int PageIndex = 1)
        {
            return SQLContext.Lite.Queryable<BangumiHisitory>()
                .WhereIF(PlayTime.HasValue, t => t.PlayTime <= PlayTime.Value)
                .OrderBy(t=>t.PlayTime,OrderByType.Desc)
                .ToList().ToPage(PageIndex, 10);
        }
        /// <summary>
        /// 删除历史纪律
        /// </summary>
        /// <returns></returns>
        public Task DeleteHisitory(int Id)
        {
            return SQLContext.Lite.Deleteable<BangumiHisitory>(t => t.Id == Id).ExecuteCommandAsync();
        }
        /// <summary>
        /// 获取历史
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public BangumiHisitory GetHistory(int Id)
        {
            return SQLContext.Lite.Queryable<BangumiHisitory>().Where(t => t.Id == Id).First();
        }


        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <returns></returns>
        public int AddCollect(BangumiCollect Collect)
        {
            return SQLContext.Lite.Insertable(Collect).ExecuteReturnIdentity();
        }
    }
}
