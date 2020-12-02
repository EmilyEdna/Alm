using Alm.Utils;
using Alm.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XExten.HttpFactory;

namespace Alm.ViewModel
{
    public class AuthorPageViewModel : ViewModelBase
    {
        public AuthorPageViewModel()
        {
            IocManager.SetCache(nameof(AuthorPageViewModel), this);
            InitSupport();
        }
        private string _Support;
        public string Support
        {
            get { return _Support; }
            set { _Support = value; OnPropertyChanged("Support"); }
        }

        private void InitSupport() {
            WebClient client = new WebClient();
            var xx = client.DownloadString("https://raw.githubusercontent.com/EmilyEdna/Alm/master/SupportList.json");
        }
    }
}
