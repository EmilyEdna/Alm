using Alm.UserControls;
using Alm.Utils;
using Alm.ViewModel.Base;
using AlmCore.Scrapy;
using AlmCore.Scrapy.EYunzhuModel;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XExten.XCore;

namespace Alm.ViewModel
{
    public class MoviePageViewModel : ViewModelBase
    {
        public MoviePageViewModel()
        {
            IocManager.SetCache(nameof(MoviePageViewModel), this);
            PageIndex = 1;
        }

        public string Kw { get; set; }
        public string ShowName { get; set; }
        private SearchRoots _Roots;
        public SearchRoots Roots
        {
            get { return _Roots; }
            set { _Roots = value; OnPropertyChanged("Roots"); }
        }
        private DetailRoots _Root;
        public DetailRoots Root
        {
            get { return _Root; }
            set { _Root = value; OnPropertyChanged("Root"); }
        }
        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; OnPropertyChanged("PageIndex"); }
        }


        #region Commands
        public Commands<string> SearchCmd => new Commands<string>((str) =>
        {
            Kw = str;
            Roots = EYunzhu.GetMovieSearch(str);
        }, null);
        public Commands<int> ShowCmd => new Commands<int>((obj) =>
         {
             Root = EYunzhu.GetMovieDetail(obj);
             ShowName = Root.Name;
         }, null);
        public Commands<FunctionEventArgs<int>> PageUpdatedCmd => new Commands<FunctionEventArgs<int>>((obj) =>
        {
            PageIndex = obj.Info;
            Roots = EYunzhu.GetMovieSearch(Kw, PageIndex);
        }, null);
        public Commands<KeyValuePair<string,string>> PlayCmd => new Commands<KeyValuePair<string, string>>((obj) =>
        {
            if (obj.Value.IsNullOrEmpty()) return;
            VLCPlay Play = new VLCPlay()
            {
                MediaURL = new Uri(obj.Value),
                BangumiName = ShowName,
                Collection = obj.Key,
                UseContinue = false
            };
            Play.Show();
        },null);
        #endregion

    }
}
