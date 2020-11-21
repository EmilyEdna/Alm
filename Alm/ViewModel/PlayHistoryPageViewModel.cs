using Alm.Pages;
using Alm.Utils;
using Alm.ViewModel.Base;
using AlmCore.SQLModel.Imomoes;
using AlmCore.SQLService;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Text;
using XExten.XCore;
using XExten.Common;

namespace Alm.ViewModel
{
    public class PlayHistoryPageViewModel : ViewModelBase
    {
        public PlayHistoryPageViewModel()
        {
            IocManager.SetCache(nameof(PlayHistoryPageViewModel), this);
            Root = ImomoeLogic.Logic.GetPlayHistories();
            PageIndex = 1;
        }

        #region Property
        private PageResult<BangumiHisitory> _Root;
        public PageResult<BangumiHisitory> Root
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
        private DateTime? _Time;
        public DateTime? Time
        {
            get { return _Time; }
            set { _Time = value; OnPropertyChanged("Time"); }
        }
        #endregion

        #region Commands
        public Commands<FunctionEventArgs<int>> PageUpdatedCmd => new Commands<FunctionEventArgs<int>>((obj) =>
        {
            PageIndex = obj.Info;
            Root = ImomoeLogic.Logic.GetPlayHistories(Time, PageIndex);
        }, null);
        public Commands<int> Delete => new Commands<int>((obj) =>
        {
            ImomoeLogic.Logic.DeleteHisitory(obj);
            Root = ImomoeLogic.Logic.GetPlayHistories(Time, PageIndex);
        }, null);
        public Commands<int> ContinuePlay => new Commands<int>((obj) =>
        {
            var History = ImomoeLogic.Logic.GetHistory(obj);
            BangumiPlay Play = new BangumiPlay
            {
                Id = History.Id,
                Collection = History.Collection,
                BangumiName = History.BangumiName,
                MediaURL = new Uri(History.BangumiURL.ToLzStringDec()),
                UseContinue = true,
                PlayProgress = TimeSpan.FromSeconds(History.SpanSeconds)
            };
            Play.Show();
        }, null);
        public Commands<object> Cmd => new Commands<object>(obj =>
        {
            var type = System.Convert.ToInt32(obj);
            if (type == 1)
                Root = ImomoeLogic.Logic.GetPlayHistories(Time, PageIndex);
            if (type == 2)
            {
                PageIndex = 1;
                Time = null;
                Root = ImomoeLogic.Logic.GetPlayHistories();
            }
            if (type == 3)
            {
                ImomoeLogic.Logic.DeleteHisitory();
                Root = ImomoeLogic.Logic.GetPlayHistories(Time, PageIndex);
            }
        }, null);
        #endregion
    }
}
