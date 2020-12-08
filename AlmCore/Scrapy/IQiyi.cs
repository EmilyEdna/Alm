using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XExten.HttpFactory;

namespace AlmCore.Scrapy
{
    public class IQiyi
    {
        private const string BaseURL = "https://so.iqiyi.com/so/q_";
        public static void GetIQiyiSearch(string Keyword) 
        {
          var data =  HttpMultiClient.HttpMulti.AddNode(BaseURL + Keyword).Build().RunString().FirstOrDefault();
        }
    }
}
