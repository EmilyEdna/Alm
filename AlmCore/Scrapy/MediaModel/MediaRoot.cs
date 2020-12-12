using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore.Scrapy.MediaModel
{
    public class MediaRoot
    {
        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public List<MediaElements> Elements { get; set; }
    }
}
