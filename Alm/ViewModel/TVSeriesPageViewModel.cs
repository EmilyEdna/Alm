using Alm.UserControls;
using Alm.Utils;
using Alm.ViewModel.Base;
using AlmCore.Scrapy;
using AlmCore.Scrapy.IQiyiModel;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using XExten.XCore;

namespace Alm.ViewModel
{
    public class TVSeriesPageViewModel : ViewModelBase
    {
        public TVSeriesPageViewModel()
        {
            IocManager.SetCache(nameof(TVSeriesPageViewModel), this);
            Type = "类型";
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
        private List<IQiyiRoot> _Root;
        public List<IQiyiRoot> Root
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
            if (Type.Equals("类型")) { Growl.Warning("请选择类型"); return; }
            if (str.IsNullOrEmpty()) { Growl.Warning("查询条件不能为空"); return; }
            if (Type.Equals("爱奇艺"))
            {
                Root = IQiyi.GetIQiyiSearch(str);
            }
        }, null);

        public Commands<IQiyiElements> PlayCmd => new Commands<IQiyiElements>((obj) =>
        {
            VLCPlay Play = new VLCPlay()
            {
                BangumiName=obj.Names,
                UseContinue=false,
                Collection=obj.Collect,
                MediaURL = new Uri(IQiyi.ResolvURL(obj.PlayUrl)),
            };
            Play.Show();
        }, null);

        #endregion
    }
}
