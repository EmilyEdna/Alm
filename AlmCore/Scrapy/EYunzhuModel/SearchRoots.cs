using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmCore.Scrapy.EYunzhuModel
{
    public class SearchRoots
    {
        [JsonProperty("total")]
        public int Total { get; set; }
        [JsonProperty("last_page")]
        public int Pages { get; set; }
        [JsonProperty("data")]
       public List<SearchElements> Elements { get; set; }
    }
}
