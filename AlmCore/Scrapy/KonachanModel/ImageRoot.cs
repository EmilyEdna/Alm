using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AlmCore.Scrapy.KonachanModel
{
    [Serializable]
    [XmlRoot("posts")]
    public class ImageRoot
    {
        /// <summary>
        /// 总页数
        /// </summary>
        public double PageTotal => Math.Ceiling(Total / 8.0);
        /// <summary>
        /// 总数
        /// </summary>
        [XmlAttribute("count")]
        public int Total { get; set; }
        [XmlElement("post")]
        public List<ImageElements> Elements { get; set; }
    }
}
