using Alm.Pages;
using Alm.Utils;
using Alm.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Alm.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            IocManager.SetCache(nameof(MainWindowViewModel), this);
        }
        private Page _CurrentPage;
        public Page CurrentPage
        {
            get
            {
                return _CurrentPage;
            }
            set
            {
                _CurrentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }
    }
}
