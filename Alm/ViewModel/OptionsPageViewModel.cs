using Alm.Utils;
using Alm.ViewModel.Base;
using AlmCore.SQLService;
using System;
using System.Collections.Generic;
using System.Text;
using XExten.XCore;
using System.Linq;
using AlmCore.SQLModel.Common;
using HandyControl.Controls;

namespace Alm.ViewModel
{
    public class OptionsPageViewModel : ViewModelBase
    {
        public OptionsPageViewModel()
        {
            IocManager.SetCache(nameof(OptionsPageViewModel), this);
            InitOptions();
        }

        #region Property
        private List<Options> _Root;
        public List<Options> Root
        {
            get { return _Root; }
            set { _Root = value; OnPropertyChanged("Root"); }
        }
        #endregion

        #region Commands
        public Commands<string> UpCmd => new Commands<string>((str) => 
        {
            Root = Root.Select(t => new Options
            {
                AddrOne = t.AddrOne,
                AddrTwo = t.AddrTwo,
                DefaultAddr = str,
                OptionPage = t.OptionPage
            }).ToList();
        }, null);
        public Commands<Options> SavCmd => new Commands<Options>((obj) => {
            if (CommonLogic.Logic.Update(obj) > 0)
                Growl.Info("配置更新完成");
        }, null);
        #endregion

        #region Method
        public void InitOptions()
        {
             Root = CommonLogic.Logic.GetOptions();
        }
        #endregion
    }
}
