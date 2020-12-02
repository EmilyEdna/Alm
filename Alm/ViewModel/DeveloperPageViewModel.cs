using Alm.Utils;
using Alm.ViewModel.Base;
using AlmCore.Scrapy;
using AlmCore.Scrapy.GithubModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alm.ViewModel
{
    public class DeveloperPageViewModel : ViewModelBase
    {
        public DeveloperPageViewModel()
        {
            IocManager.SetCache(nameof(DeveloperPageViewModel), this);
            Root = Github.GetDevelopers();
        }

        private List<DeveloperRoot> _Root;
        public List<DeveloperRoot> Root
        {
            get { return _Root; }
            set { _Root = value; OnPropertyChanged("Root"); }
        }
    }
}
