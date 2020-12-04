using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlmUpdate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Extension util = new Extension();
            util.GetFilesList();
        }

        private void OpenAlm() {
            Process process = new Process();
            process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "Alm.exe";
            process.StartInfo.CreateNoWindow = true;
            process.Start();//启动
            process.CloseMainWindow();//通过向进程的主窗口发送关闭消息来关闭拥有用户界面的进程
            process.Close();//释放与此组件关联的所有资源
            Environment.Exit(0);
        }
    
    }
}
