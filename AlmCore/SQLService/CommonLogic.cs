using AlmCore.SQLModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore.SQLService
{
    public class CommonLogic
    {
        public static CommonLogic Logic => new CommonLogic();
        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public int Update(Options Data)
        {
            return SQLContext.Lite.Updateable<Options>()
                .SetColumns(t=>t.OptionPage==Data.OptionPage)
                .SetColumns(t=>t.DefaultAddr==Data.DefaultAddr)
                .Where(t=>t.Id==1)
                .ExecuteCommand();
        }
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        public List<Options> GetOptions()
        {
            var Count = SQLContext.Lite.Queryable<Options>().Count();
            if (Count == 0)
            {
                SQLContext.Lite.Insertable(new Options
                {
                    OptionPage = 3,
                    AddrOne = "https://vip.52jiexi.top/?url=",
                    AddrTwo = "https://cdn.yangju.vip/kc/api.php?url=",
                    DefaultAddr = "https://vip.52jiexi.top/?url="
                }).ExecuteCommand();
            }
            return SQLContext.Lite.Queryable<Options>().ToList();
        }
    }
}
