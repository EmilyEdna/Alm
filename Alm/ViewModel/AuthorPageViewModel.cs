using Alm.Utils;
using Alm.ViewModel.Base;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XExten.HttpFactory;
using XExten.XCore;
using System.Linq;
using AlmCore.Scrapy;

namespace Alm.ViewModel
{
    public class AuthorPageViewModel : ViewModelBase
    {
        public AuthorPageViewModel()
        {
            IocManager.SetCache(nameof(AuthorPageViewModel), this);
            Support = string.Join(",", Github.GetSupport().DataList);
        }
        private string _Support;
        public string Support
        {
            get { return _Support; }
            set { _Support = value; OnPropertyChanged("Support"); }
        }
    }
}
