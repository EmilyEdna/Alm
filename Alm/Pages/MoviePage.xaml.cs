using AlmCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Alm.Pages
{
    /// <summary>
    /// MoviePage.xaml 的交互逻辑
    /// </summary>
    public partial class MoviePage : Page
    {
        public MoviePage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vlcCtrl.SourceProvider.CreatePlayer(Extension.VLCPath);

            vlcCtrl.SourceProvider.MediaPlayer.Play(new Uri ("D:\\Movie\\[LoliHouse] Tenki No Ko [BDRip 1920x1080 HEVC-10bit FLAC PGS(chs,eng,jpn)].mkv"));
        }
    }
}
