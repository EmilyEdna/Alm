using Alm.Pages;
using Alm.Pages.Confirms;
using Alm.Utils;
using Alm.ViewModel.Base;
using Alm.ViewModel.Confirms;
using AlmCore.SQLModel.Konachans;
using AlmCore.SQLService;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using XExten.XCore;

namespace Alm.ViewModel
{
    public class KonachanLabelViewModel : ViewModelBase
    {
        public KonachanLabelViewModel()
        {
            IocManager.SetCache(nameof(KonachanLabelViewModel), this);
            PageIndex = 1;
            PageIndexs = 1;
            var Total = 0;
            var UTotal = 0;
            Root = KonachanLogic.Logic.GetTags(ref Total);
            URoot = KonachanLogic.Logic.GetUserTagsPage(ref UTotal);
            RTotal = (int)Math.Ceiling(Total / 20.0);
            URTotal = (int)Math.Ceiling(UTotal / 20.0);
        }

        #region Property
        private List<Tags> _Root;
        public List<Tags> Root
        {
            get { return _Root; }
            set { _Root = value; OnPropertyChanged("Root"); }
        }

        private List<UserTags> _URoot;
        public List<UserTags> URoot
        {
            get { return _URoot; }
            set { _URoot = value; OnPropertyChanged("URoot"); }
        }
        private int _RTotal;
        public int RTotal
        {
            get { return _RTotal; }
            set { _RTotal = value; OnPropertyChanged("RTotal"); }
        }
        private int _URTotal;
        public int URTotal
        {
            get { return _URTotal; }
            set { _URTotal = value; OnPropertyChanged("URTotal"); }
        }
        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; OnPropertyChanged("PageIndex"); }
        }
        private int _PageIndexs;
        public int PageIndexs
        {
            get { return _PageIndexs; }
            set { _PageIndexs = value; OnPropertyChanged("PageIndexs"); }
        }
        private string _DiyName;
        public string DiyName
        {
            get { return _DiyName; }
            set { _DiyName = value; OnPropertyChanged("DiyName"); }
        }
        private string _DiyValue;
        public string DiyValue
        {
            get { return _DiyValue; }
            set { _DiyValue = value; OnPropertyChanged("DiyValue"); }
        }
        #endregion

        #region Commands

        public Commands<FunctionEventArgs<int>> PageUpdatedCmdUR => new Commands<FunctionEventArgs<int>>((obj) =>
        {
            PageIndexs = obj.Info;
            var UTotal = 0;
            URoot = KonachanLogic.Logic.GetUserTagsPage(ref UTotal, PageIndexs);
        }, null);
        public Commands<FunctionEventArgs<int>> PageUpdatedCmdR => new Commands<FunctionEventArgs<int>>((obj) =>
        {
            PageIndex = obj.Info;
            var Total = 0;
            Root = KonachanLogic.Logic.GetTags(ref Total, PageIndex);
        }, null);
        public Commands<string> AddCmd => new Commands<string>((str) =>
        {
            var UTotal = 0;
            URoot = KonachanLogic.Logic.GetUserTagsPage(ref UTotal);
        }, null);

        public Commands<int> RmCmd => new Commands<int>((obj) =>
         {
             KonachanLogic.Logic.RemoveUserTags(obj);
             var UTotal = 0;
             URoot = KonachanLogic.Logic.GetUserTagsPage(ref UTotal);
         }, null);
        public Commands<object> DiyCmd => new Commands<object>((obj) =>
        {
            if (!DiyName.IsNullOrEmpty() && !DiyValue.IsNullOrEmpty())
            {
                KonachanLogic.Logic.AddUserTags(new UserTags
                {
                    AddTime = DateTime.Now,
                    DiyName = DiyName,
                    DiyValue = DiyValue
                });
                var UTotal = 0;
                URoot = KonachanLogic.Logic.GetUserTagsPage(ref UTotal);
                DiyName = null;
                DiyValue = null;
            }
        }, null);
        #endregion
    }
}
