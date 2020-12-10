using Alm.Controls.Base;
using Alm.Utils;
using Alm.ViewModel;
using AlmCore.SQLService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Vlc.DotNet.Wpf;

namespace Alm.Controls
{
    public class AlmBtn : ButtonBase
    {
        Window targetWindow;
        public AlmBtn()
        {
            ButtonHoverColor = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            Click += AlmBtn_Click;
        }

        public bool IsMax
        {
            get { return (bool)GetValue(IsMaxProperty); }
            set { SetValue(IsMaxProperty, value); }
        }

        public static readonly DependencyProperty IsMaxProperty =
            DependencyProperty.Register("IsMax", typeof(bool), typeof(AlmBtn), new PropertyMetadata(false));

        private void AlmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (targetWindow == null)
                targetWindow = Window.GetWindow(this);
            AlmBtn btn = sender as AlmBtn;
            var Param = btn.CommandParameter.ToString();
            if (Param.Equals("Close"))
            {
                List<VlcControl> VCtrls = IocManager.GetChildObjects<VlcControl>(targetWindow, "VCtrl");
                if (VCtrls.Count != 0)
                {
                    VCtrls.ForEach(item =>
                    {
                        Task.Run(() =>
                        {
                            if (item.SourceProvider.MediaPlayer != null)
                                item.SourceProvider.MediaPlayer.Stop();
                        });
                    });
                }
                targetWindow.Close();
            }
            if (Param.Equals("Min"))
                targetWindow.WindowState = WindowState.Minimized;
            if (Param.Equals("Max"))
            {
                targetWindow.StateChanged += (sender, e) =>
                {
                    if (targetWindow.WindowState == WindowState.Normal) IsMax = false;
                    if (targetWindow.WindowState == WindowState.Maximized) IsMax = true;
                };
                if (targetWindow.WindowState == WindowState.Normal)
                    targetWindow.WindowState = WindowState.Maximized;
                else if (targetWindow.WindowState == WindowState.Maximized)
                    targetWindow.WindowState = WindowState.Normal;
            }
        }


    }
}
