using Alm.Utils;
using Alm.ViewModel.Base;
using AlmCore.Scrapy.KonachanModel;
using AlmCore.SQLModel.Konachans;
using AlmCore.SQLService;
using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Text;
using XExten.Common;
using XExten.XCore;

namespace Alm.ViewModel
{
    public class CollectPageViewModel : ViewModelBase
    {
        public CollectPageViewModel()
        {
            IocManager.SetCache(nameof(CollectPageViewModel), this);
            PageIndex = 1;
            Init();
        }

        #region Property

        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; OnPropertyChanged("PageIndex"); }
        }
        private PageResult<KonaCollect> _Root;
        public PageResult<KonaCollect> Root
        {
            get { return _Root; }
            set { _Root = value; OnPropertyChanged("Root"); }
        }
        private DateTime? _Time;
        public DateTime? Time
        {
            get { return _Time; }
            set { _Time = value; OnPropertyChanged("Time"); }
        }
        #endregion

        #region Methond
        public void Init()
        {
            Root = KonachanLogic.Logic.GetCollect(Time, PageIndex);
        }
        #endregion

        #region Commands
        public Commands<FunctionEventArgs<int>> PageUpdatedCmd => new Commands<FunctionEventArgs<int>>((obj) =>
        {
            PageIndex = obj.Info;
            Root = KonachanLogic.Logic.GetCollect(Time, PageIndex);
        }, null);
        public Commands<object> SearchCmd => new Commands<object>((obj) =>
        {
            PageIndex = 1;
            Root = KonachanLogic.Logic.GetCollect(Time, PageIndex);
        }, null);

        public Commands<ImageElements> DownCmd => new Commands<ImageElements>((obj) =>
        {
            if (KonachanLogic.Logic.CheckRecord(obj.Id))
            {
                KonachanLogic.Logic.AddDownRecord(new DownRecord
                {
                    DownTime = DateTime.Now,
                    FileURL = obj.FileURL,
                    Id = obj.Id,
                    Name = obj.Tag,
                    State = "未开始"
                });
                Growl.Info("已加入到下载列表");
            }
            else Growl.Info("已经在下载列表了");
        }, null);

        public Commands<long> RemoveCmd => new Commands<long>((obj) =>
        {
            KonachanLogic.Logic.RemoveCollect(obj);
            Init();
        }, null);
        #endregion
    }
}
