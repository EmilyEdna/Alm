using Alm.Utils.Enums;
using AlmCore.SQLModel.Imomoes;
using AlmCore.SQLService;
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
using XExten.XCore;

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
        #region Property
        public bool UseContinue { get; set; }
        public string BangumiName { get; set; }
        public string Collection { get; set; }
        public Uri MediaURL { get; set; }
        public int Id { get; set; }
        public TimeSpan PlayProgress { get; set; }
        #endregion
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
                    if (UseContinue)
                        BangumiMedia.Position = PlayProgress;
                    Rate.Maximum = BangumiMedia.NaturalDuration.TimeSpan.TotalSeconds;
                    RateTotal.Text = $"/{BangumiMedia.NaturalDuration.TimeSpan.ToString().Split(".")[0]}";
                    if (ImomoeLogic.Logic.CheckHistory(BangumiName, Collection))
                    {
                        Id = ImomoeLogic.Logic.AddHistory(new BangumiHisitory
                        {
                            BangumiName = BangumiName,
                            Collection = Collection,
                            BangumiURL = MediaURL.ToString().ToLzStringEnc(),
                            PlayProgress = "0",
                            PlayTime = DateTime.Now
                        }); ;
                    }
                };
                timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };
                timer.Tick += new EventHandler((obj, e) =>
                {
                    Rate.Value = BangumiMedia.Position.TotalSeconds;
                    RatePlay.Text = BangumiMedia.Position.ToString().Split(".")[0];
                    if (Convert.ToInt32(BangumiMedia.Position.TotalSeconds.ToString().Split(".")[0]) % 60 == 0)
                        ImomoeLogic.Logic.UpdateHistory(Id, RatePlay.Text, BangumiMedia.Position.TotalSeconds);
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
