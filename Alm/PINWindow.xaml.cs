using System;
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
                DialogResult = true;
            }
            else
            {
                ErrorInfo.Text = "解锁码错误!";
                return;
            }
        }

        private void AlmWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
