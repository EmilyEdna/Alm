using Alm.Pages;
using Alm.Utils;
using Alm.ViewModel.Base;
using AlmCore;
using AlmCore.Scrapy;
using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Alm.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            IocManager.SetCache(nameof(MainWindowViewModel), this);
            Ver = Application.ResourceAssembly.GetName().Version.ToString();
            CheckVersion();
            CurrentPage = new AuthorPage();
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

        private string _Ver;
        public string Ver
        {
            get { return _Ver; }
            set { _Ver = value; OnPropertyChanged("Ver"); }
        }

        public void CheckVersion()
        {
            var NewVer = Github.GetVersion().Replace("\n", "");
            var New = int.Parse(NewVer.Replace(".", ""));
            var Old = int.Parse(Ver.Replace(".", ""));
            if (Old< New)
            {
                var result = HandyControl.Controls.MessageBox.Show(new MessageBoxInfo
                {
                    Button = MessageBoxButton.OK,
                    DefaultResult = MessageBoxResult.OK,
                    Message = "检测到新版本，即将升级"
                });
                if (result == MessageBoxResult.OK)
                {
                    Process process = new Process();
                    process.StartInfo.FileName = Extension.ApplicationRoute + "AlmUpdate.exe";
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();//启动
                    process.CloseMainWindow();//通过向进程的主窗口发送关闭消息来关闭拥有用户界面的进程
                    process.Close();//释放与此组件关联的所有资源
                    Environment.Exit(0);
                }
            }
        }
    }
}
