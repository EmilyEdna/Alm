using Alm.Controls.Base;
using Alm.Utils;
using Alm.ViewModel;
using AlmCore.Scrapy.KonachanModel;
using AlmCore.SQLService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

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
                targetWindow.Close();
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
            if (btn.CommandParameter.GetType() == typeof(ImageElements))
            {
                ResourceDictionary dictionary = new ResourceDictionary()
                {
                    Source = new Uri(@"/Alm;component/Style/Geometry.xaml", UriKind.Relative)
                };
                ImageElements Elements = (btn.CommandParameter as ImageElements);
                if (KonachanLogic.Logic.HasCollect(Elements.Id))
                {
                    KonachanLogic.Logic.AddCollect(Elements);
                    btn.Icon = Geometry.Parse(dictionary["YesStarIcon"].ToString());
                }
                else
                {
                    KonachanLogic.Logic.RemoveCollect(Elements.Id);
                    btn.Icon = Geometry.Parse(dictionary["NoStarIcon"].ToString());
                }
            }
            if (btn.CommandParameter.GetType() == typeof(long)) 
            {
                var Id = Convert.ToInt64(btn.CommandParameter);
                KonachanLogic.Logic.RemoveCollect(Id);
                CollectPageViewModel vm = IocManager.GetCache<CollectPageViewModel>(nameof(CollectPageViewModel));
                vm.Init();
            }
        }
    }
}
