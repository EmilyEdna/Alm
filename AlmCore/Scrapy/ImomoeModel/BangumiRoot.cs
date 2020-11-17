using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore.Scrapy.ImomoeModel
{
    public class BangumiRoot
    {
        public int TotalPage { get; set; }
        public int Total { get; set; }
        public List<BangumiElements> Elements { get; set; }
    }
}
