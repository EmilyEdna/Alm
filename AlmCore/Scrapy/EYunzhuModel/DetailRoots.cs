using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore.Scrapy.EYunzhuModel
{
    public class DetailRoots
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("pic")]
        public string Cover { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("playUrl")]
        public Dictionary<string,string> PlayURL { get; set; }
    }
}
