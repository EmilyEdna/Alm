using Alm.Utils;
using Alm.ViewModel.Base;
using AlmCore.Scrapy;
using AlmCore.Scrapy.KonachanModel;
using AlmCore.SQLModel.Konachans;
using AlmCore.SQLService;
using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alm.ViewModel
{
    public class KonachanPageViewModel: ViewModelBase
    {
        public KonachanPageViewModel()
        {
            IocManager.SetCache(nameof(KonachanPageViewModel), this);
            Type = "类型";
            PageIndex = 1;
            Root = Konachan.GetImage();
            Tags = KonachanLogic.Logic.GetUserTags();
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
        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; OnPropertyChanged("PageIndex"); }
        }
        private ImageRoot _Root;
        public ImageRoot Root {
            get { return _Root; }
            set { _Root = value; OnPropertyChanged("Root"); }
        }
        private List<UserTags> _Tags;
        public List<UserTags> Tags
        {
            get { return _Tags; }
            set { _Tags = value; OnPropertyChanged("Tags"); }
        }
        #endregion

        #region Commands
        public Commands<FunctionEventArgs<int>> PageUpdatedCmd => new Commands<FunctionEventArgs<int>>((obj) =>
        {
            PageIndex = obj.Info;
            Root = Konachan.GetImage(PageIndex,null, ex => Growl.Error(ex.Message));
        }, null);
        public Commands<object> SearchCmd => new Commands<object>((obj) =>
        {
        }, null);
        #endregion
    }
}
