using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AlmCore.Scrapy.KonachanModel
{
    [Serializable]
    [XmlRoot("tags")]
    public class TagRoot
    {
        /// <summary>
        /// 标签
        /// </summary>
        [XmlElement("tag")]
        public List<TagElements> Elements { get; set; }
    }
}
