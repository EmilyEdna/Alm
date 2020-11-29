using Alm.Utils;
using Alm.Utils.Enums;
using Alm.ViewModel.Base;
using AlmCore;
using AlmCore.Downer;
using AlmCore.SQLModel.Konachans;
using AlmCore.SQLService;
using AlmCore.ThreadDowner;
using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XExten.Common;
using XExten.XCore;

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
        private DownManager down;
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
            if (type == 4)
            {
                Process.Start("explorer.exe", Extension.CreateDir(Extension.SavrDir));
            }
        }, null);

        public Commands<Dictionary<DownloadEnum, DownRecord>> HanderCmd => new Commands<Dictionary<DownloadEnum, DownRecord>>((obj) =>
        {
            DownRecord Record = obj.Values.FirstOrDefault();
            switch (obj.Keys.FirstOrDefault())
            {
                case DownloadEnum.Start:
                    var Info = ThreadMainCore.Instance.MainHttp(Record.FileURL.ToLzStringDec());
                    ThreadPoolCore.Run = true;
                    ThreadPoolCore.Instance.CreateTaskInfo(Info);
                    break;
                case DownloadEnum.ReStart:
                    if (down == null) Init();
                    down.ReStart();
                    break;
                case DownloadEnum.Delete:
                    KonachanLogic.Logic.DeleteRecord(Record.Id);
                    Root = KonachanLogic.Logic.GetDownRecord(Time, PageIndex);
                    break;
                case DownloadEnum.Stop:
                    down?.Stop();
                    break;
                default:
                    break;
            }
        }, null);

        public Commands<FunctionEventArgs<int>> PageUpdatedCmd => new Commands<FunctionEventArgs<int>>((obj) =>
        {
            PageIndex = obj.Info;
            Root = KonachanLogic.Logic.GetDownRecord(Time, PageIndex);
        }, null);
        #endregion


        #region Private
        private void Init()
        {
           
            DownInfo Info = new DownInfo
            {
                SaveDir = Extension.CreateDir(Extension.SavrDir),
                DownloadUrlList = new List<string> {  },
                TaskCount = 2
            };
            down = new DownManager(Info);
            down.OnDownload += Down_OnDownload;
            down.OnStart += Down_OnStart;
            down.OnStop += Down_OnStop;
            down.OnFinsh += Down_OnFinsh;
        }

        private void Down_OnFinsh()
        {
            Growl.Info("下载完成");
        }

        private void Down_OnStop()
        {
            Growl.Info("暂停下载");
        }

        private void Down_OnStart()
        {
            Growl.Info("开始下载");
        }

        private void Down_OnDownload(long arg1, long arg2)
        {
            var xx = arg1;
        }
        #endregion
    }
}
