using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore.Scrapy.ImomoeModel
{
    public class BangumiDetailRoot
    {
        /// <summary>
        /// 封面
        /// </summary>
        public string Conver { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string Description { get; set; }
        public List<BangumiDetailElements> Elements { get; set; }
    }
}
