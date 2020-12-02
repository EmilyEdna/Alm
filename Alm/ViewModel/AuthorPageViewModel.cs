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

        private void InitSupport()
        {
            var Result = new WebClient().DownloadString("https://raw.githubusercontent.com/EmilyEdna/Alm/master/SupportList.json");
            Support = string.Join(",", Result.ToModel<JObject>().SelectToken("DataList").Children().Select(t => t.ToString()));
        }
    }
}
