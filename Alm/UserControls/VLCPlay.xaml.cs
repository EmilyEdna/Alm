using Alm.Utils.Enums;
using AlmCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Vlc.DotNet.Core;

namespace Alm.UserControls
{
    /// <summary>
    /// VLCPlay.xaml 的交互逻辑
    /// </summary>
    public partial class VLCPlay
    {
        public Uri MediaURL { get; set; }
        private bool IsPlayed = false;
        public VLCPlay()
        {
            InitializeComponent();

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (sender as Button);
            MediaEnum media = Enum.Parse<MediaEnum>(btn.CommandParameter.ToString());
            switch (media)
            {
                case MediaEnum.Play:
                    if (IsPlayed)
                    {
                        VCtrl.SourceProvider.MediaPlayer.Play();
                    }
                    else
                    {
                        VCtrl.SourceProvider.CreatePlayer(Extension.VLCPath);
                        VCtrl.SourceProvider.MediaPlayer.Play(MediaURL);
                        VCtrl.SourceProvider.MediaPlayer.Playing += MediaPlayer_Playing;
                        VCtrl.SourceProvider.MediaPlayer.PositionChanged += MediaPlayer_PositionChanged;
                        IsPlayed = true;
                    }
                    break;
                case MediaEnum.Pause:
                    VCtrl.SourceProvider.MediaPlayer.Pause();
                    break;
                case MediaEnum.Stop:
                    Task.Run(() => VCtrl.SourceProvider.MediaPlayer.Stop());
                    break;
                default:
                    break;
            }
        }
        private void MediaPlayer_Playing(object sender, VlcMediaPlayerPlayingEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                var play = (sender as VlcMediaPlayer);
                Rate.Maximum = play.Length/1000;
                RateTotal.Text = TimeSpan.FromSeconds(play.Length / 1000).ToString();
            });
        }
        private void MediaPlayer_PositionChanged(object sender, VlcMediaPlayerPositionChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Rate.Value = Convert.ToDouble(e.NewPosition* 1000000);
                var play = (sender as VlcMediaPlayer);
                Rate.Value = play.Time/1000;
                RatePlay.Text = TimeSpan.FromSeconds(play.Time / 1000).ToString() + "/";
            });
        }
        private void Voice_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (VCtrl.SourceProvider.MediaPlayer == null) return;
            VCtrl.SourceProvider.MediaPlayer.Audio.Volume = Convert.ToInt32(e.NewValue) * 10;
        }
        private void Rate_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            if (VCtrl.SourceProvider.MediaPlayer == null) return;
            var position = (float)(Rate.Value / Rate.Maximum);
            if (position == 1)
            {
                position = 0.99f;
            }
            VCtrl.SourceProvider.MediaPlayer.Position = position;
        }
    }
}
