using Alm.Utils.Enums;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Alm.Pages
{
    /// <summary>
    /// BangumiPlay.xaml 的交互逻辑
    /// </summary>
    public partial class BangumiPlay
    {
        public BangumiPlay()
        {
            InitializeComponent();
        }
        public Uri MediaURL { get; set; }
        DispatcherTimer timer = null;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BangumiMedia.Source = MediaURL;
            Button btn = (sender as Button);
            MediaEnum media = Enum.Parse<MediaEnum>(btn.CommandParameter.ToString());
            if (media == MediaEnum.Pause)
                BangumiMedia.Pause();
            if (media == MediaEnum.Stop)
                BangumiMedia.Stop();
            if (media == MediaEnum.Play)
            {
                BangumiMedia.Play();
                BangumiMedia.MediaOpened += (obj, e) =>
                {
                    Rate.Maximum = BangumiMedia.NaturalDuration.TimeSpan.TotalSeconds;
                    RateTotal.Text = $"/{BangumiMedia.NaturalDuration.TimeSpan.ToString().Split(".")[0]}";
                };
                timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };
                timer.Tick += new EventHandler((obj, e) =>
                {
                    Rate.Value = BangumiMedia.Position.TotalSeconds;
                    RatePlay.Text = BangumiMedia.Position.ToString().Split(".")[0];
                });
                timer.Start();
            }
        }

        private void Rate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BangumiMedia.Position = TimeSpan.FromSeconds(Rate.Value);
        }
    }
}
