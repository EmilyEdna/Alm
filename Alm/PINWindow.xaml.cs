using AlmCore.Scrapy;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using XExten.XCore;

namespace Alm
{
    /// <summary>
    /// PINWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PINWindow
    {
        public PINWindow()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Pwd.Password.IsNullOrEmpty())
            {
                ErrorInfo.Text = "解锁码错误!";
                return;
            }
            var pin = DateTime.Now.ToString("yyyyMMdd");
            if (Pwd.Password.Equals(pin))
            {
                new MainWindow().Show();
                Close();
            }
            else
            {
                ErrorInfo.Text = "解锁码错误!";
                return;
            }
        }
    }
}
