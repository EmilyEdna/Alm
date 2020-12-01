using Alm.Utils;
using Alm.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alm.ViewModel.Confirms
{
    public class ConfirmViewModel: ViewModelBase
    {
        public ConfirmViewModel()
        {
            IocManager.SetCache(nameof(ConfirmViewModel), this);
        }
        private string _Result;
        public string Result 
        {
            get { return _Result; }
            set { _Result = value;OnPropertyChanged("Result"); }
        }
    }
}
