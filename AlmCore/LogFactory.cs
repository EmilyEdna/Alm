using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore
{
    public class LogFactory
    {
        private static Logger loger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteLog(Exception ex)
        {
            loger.Error(ex);
        }
    }
}
