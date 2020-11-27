using Alm.Utils;
using Alm.Utils.Enums;
using Alm.ViewModel.Base;
using AlmCore;
using AlmCore.Downer;
using AlmCore.SQLModel.Konachans;
using AlmCore.SQLService;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XExten.Common;

namespace Alm.ViewModel
{
    public class DownRecordPageViewModel : ViewModelBase
    {
        public DownRecordPageViewModel()
        {
            IocManager.SetCache(nameof(DownRecordPageViewModel), this);
            PageIndex = 1;
            Root = KonachanLogic.Logic.GetDownRecord();
        }
        #region Property
        private PageResult<DownRecord> _Root;
        public PageResult<DownRecord> Root
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

        public Commands<object> Cmd => new Commands<object>(obj =>
        {
            var type = System.Convert.ToInt32(obj);
            if (type == 1)
                Root = KonachanLogic.Logic.GetDownRecord(Time, PageIndex);
            if (type == 2)
            {
                PageIndex = 1;
                Time = null;
                Root = KonachanLogic.Logic.GetDownRecord();
            }
            if (type == 3)
            {
                KonachanLogic.Logic.DeleteRecordAll();
                Root = KonachanLogic.Logic.GetDownRecord(Time, PageIndex);
            }
        }, null);

        public Commands<Dictionary<DownloadEnum, DownRecord>> HanderCmd => new Commands<Dictionary<DownloadEnum, DownRecord>>((obj) =>
        {
            DownInfo Info = new DownInfo
            {
                SaveDir = Extension.CreateDir(Extension.SavrDir),
                DownloadUrlList = new List<string> { obj.Values.FirstOrDefault().FileURL },
                TaskCount = 4
            };
            DownManager down = new DownManager(Info);
            down.Start();
        }, null);

        public Commands<FunctionEventArgs<int>> PageUpdatedCmd => new Commands<FunctionEventArgs<int>>((obj) =>
        {
            PageIndex = obj.Info;
            Root = KonachanLogic.Logic.GetDownRecord(Time, PageIndex);
        }, null);
        #endregion
    }
}
