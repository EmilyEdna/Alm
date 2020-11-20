using Alm.Utils;
using Alm.ViewModel.Base;
using AlmCore.Scrapy;
using AlmCore.Scrapy.ImomoeModel;
using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Text;
using XExten.XCore;

namespace Alm.ViewModel
{
    public class BangumiPageViewModel : ViewModelBase
    {
        public BangumiPageViewModel()
        {
            IocManager.SetCache(nameof(BangumiPageViewModel), this);
            Type = "类型";
            PageIndex = 1;
        }

        #region Property
        public object WordCache { get; set; }
        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; OnPropertyChanged("PageIndex"); }
        }
        private string _Type;
        public string Type
        {
            get { return _Type; }
            set { _Type = value; OnPropertyChanged("Type"); }
        }
        private string _Info;
        public string Info
        {
            get { return _Info; }
            set { _Info = value; OnPropertyChanged("Info"); }
        }
        private string _KeyWord;
        public string KeyWord
        {
            get { return _KeyWord; }
            set { _KeyWord = value; OnPropertyChanged("KeyWord"); }
        }
        private BangumiRoot _Root;
        public BangumiRoot Root
        {
            get { return _Root; }
            set { _Root = value; OnPropertyChanged("Root"); }
        }
        private BangumiDetailRoot _BRoot;
        public BangumiDetailRoot BRoot
        {
            get { return _BRoot; }
            set { _BRoot = value; OnPropertyChanged("BRoot"); }
        }
        #endregion

        #region Commands
        public Commands<string> ShowCmd => new Commands<string>((str) =>
        {
            BRoot = Imomoe.GetBangumiPage(str, ex => Growl.Error(ex.Message));
            if (BRoot != null)
                Info = "介绍";
        }, null);

        public Commands<string> SelectCmd => new Commands<string>((str) =>
        {
            Type = str;
        }, null);

        public Commands<string> SearchCmd => new Commands<string>((str) =>
        {
            Root = null;
            if (Type.Equals("类型")) { Growl.Warning("请选择类型"); return; }
            if (str.IsNullOrEmpty()) { Growl.Warning("查询条件不能为空"); return; }
            if (Type.Equals("名称")) { Root = Imomoe.GetBangumi(str); WordCache = str; }
            else
            {
                int.TryParse(str, out int Year);
                if (Year == 0) { Growl.Warning("请输入正确年份"); return; }
                Root = Imomoe.GetBangumi(Year);
                WordCache = Year;
            }
        }, null);

        public Commands<FunctionEventArgs<int>> PageUpdatedCmd => new Commands<FunctionEventArgs<int>>((obj) =>
        {
            PageIndex = obj.Info;
            Root = Imomoe.GetBangumi(WordCache, PageIndex, ex => Growl.Error(ex.Message));
        }, null);
        #endregion
    }
}
