using AlmCore.Scrapy;
using HandyControl.Controls;
using System;
using System.Threading.Tasks;
using System.Timers;
using XExten.XPlus;

namespace Alm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Timer timer = new Timer
            {
                Interval = 60000,
                AutoReset = false,
                Enabled = false,
            };
            timer.Elapsed += (s, e) =>
            {
                Task.Factory.StartNew(() => XPlusEx.XTry(() => Konachan.InitTags(), ex => Growl.Info("网络连接失败!")));
            };
            timer.Start();
        }
    }
}
