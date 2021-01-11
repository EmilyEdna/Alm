using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AlmCore.Scrapy.MusicModel
{
    public class NetEasyCloundMusic
    {
        private static readonly string BaseURL = "https://music.163.com/weapi/";
        public static readonly string Search = Path.Combine(BaseURL, "search/suggest/multimatch");
    }
}
