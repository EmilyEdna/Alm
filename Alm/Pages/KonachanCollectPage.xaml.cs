using Alm.Utils;
using AlmCore;
using AlmCore.SQLModel.Konachans;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Alm.Pages
{
    /// <summary>
    /// KonachanCollectPage.xaml 的交互逻辑
    /// </summary>
    public partial class KonachanCollectPage : Page
    {
        public KonachanCollectPage()
        {
            InitializeComponent();
        }

        private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement CopyFrame = (sender as FrameworkElement);
            KonaCollect Kona = CopyFrame.DataContext as KonaCollect;
            var FileName = Kona.FileURL.Split("/").LastOrDefault();
            var bytes = new WebClient().DownloadData(Kona.FileURL);
            FileWatcher.Instance.StartWatcherFile();
            using var Fs = File.Create(Path.Combine(Extension.SavrDir, FileName));
            Fs.Write(bytes, 0, bytes.Length);
            Fs.Close();
            Fs.Dispose();
            DataObject dragObj = new DataObject();
            dragObj.SetFileDropList(new StringCollection() { Path.Combine(Extension.SavrDir, FileName) });
            DragDrop.DoDragDrop(CopyFrame, dragObj, DragDropEffects.Copy);
            FileWatcher.Instance.StopWatchFile();
        }
    }
}
