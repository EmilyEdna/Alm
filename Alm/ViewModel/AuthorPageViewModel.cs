using Alm.Utils;
using Alm.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alm.ViewModel
{
    public class AuthorPageViewModel : ViewModelBase
    {
        public AuthorPageViewModel()
        {
            IocManager.SetCache(nameof(AuthorPageViewModel), this);
        }
        private string _Support;
        public string Support
        {
            get { return _Support; }
            set { _Support = value; OnPropertyChanged("Support"); }
        }
    }
}
