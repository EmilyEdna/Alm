using Alm.UserControls;
using Alm.Utils;
using Alm.ViewModel.Base;
using AlmCore;
using AlmCore.Scrapy;
using AlmCore.Scrapy.MediaModel;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XExten.XCore;

namespace Alm.ViewModel
{
    public class TVSeriesPageViewModel : ViewModelBase
    {
        public TVSeriesPageViewModel()
        {
            IocManager.SetCache(nameof(TVSeriesPageViewModel), this);
            Type = "频道";
        }
        #region Property
        private string _Type;
        public string Type
        {
            get { return _Type; }
            set { _Type = value; OnPropertyChanged("Type"); }
        }
        private string _KeyWord;
        public string KeyWord
        {
            get { return _KeyWord; }
            set { _KeyWord = value; OnPropertyChanged("KeyWord"); }
        }
        private List<MediaRoot> _Root;
        public List<MediaRoot> Root
        {
            get => _Root;
            set
            {
                _Root = value;
                OnPropertyChanged("Root");
            }
        }
        #endregion

        #region Commands
        public Commands<string> SelectCmd => new Commands<string>((str) =>
        {
            Type = str;
        }, null);

        public Commands<string> SearchCmd => new Commands<string>((str) =>
        {
            if (Type.Equals("频道")) { Growl.Warning("请选择频道"); return; }
            if (str.IsNullOrEmpty()) { Growl.Warning("查询条件不能为空"); return; }
            if (Type.Equals("爱奇艺"))
            {
                Root = IQiyi.GetIQiyiSearch(str,ex=>Growl.Error("未找到相关资源"));
            }
            else
            {
                Root = Tencent.GetTencentSearch(str, ex => Growl.Error("未找到相关资源"));
            }
        }, null);

        public Commands<MediaElements> PlayCmd => new Commands<MediaElements>((obj) =>
        {
            if (obj.PlayUrl.IsNullOrEmpty()) {
                Growl.Warning("暂无资源");
                return;
            }
            var URL = Extension.ResolvURL(obj.PlayUrl);
            if (URL.IsNullOrEmpty())
            {
                Growl.Warning("解析地址失效");
                return;
            }
            VLCPlay Play = new VLCPlay()
            {
                BangumiName = obj.Names,
                UseContinue = false,
                Collection = obj.Collect,
                MediaURL = new Uri(URL),
            };
            Play.Show();
        }, null);
        public Commands<MediaElements> DownCmd => new Commands<MediaElements>((obj) => {
            if (obj.PlayUrl.IsNullOrEmpty())
            {
                Growl.Warning("暂无资源");
                return;
            }
            var URL = Extension.ResolvURL(obj.PlayUrl);
            if (URL.IsNullOrEmpty())
            {
                Growl.Warning("解析地址失效");
                return;
            }
        }, null);
        #endregion
    }
}
