using AlmCore.Scrapy;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
