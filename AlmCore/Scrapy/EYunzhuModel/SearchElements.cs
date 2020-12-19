using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore.Scrapy.EYunzhuModel
{
    public class SearchElements
    {
        [JsonProperty("vid")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("pic")]
        public string Cover { get; set; }
    }
}
