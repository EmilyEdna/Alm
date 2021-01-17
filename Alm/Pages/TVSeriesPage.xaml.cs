using Alm.Utils;
using Alm.ViewModel;
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
    /// TVSeriesPage.xaml 的交互逻辑
    /// </summary>
    public partial class TVSeriesPage : Page
    {
        public TVSeriesPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (e.OriginalSource as Button);
            TVSeriesPageViewModel VM = IocManager.GetCache<TVSeriesPageViewModel>(nameof(TVSeriesPageViewModel));
            var menu = btn.ContextMenu;
            menu.PlacementTarget = btn;
            menu.IsOpen = true;
            foreach (MenuItem item in menu.Items)
            {
                item.CommandParameter = btn.CommandParameter;
                if (item.Header.Equals("播放"))
                    item.Command = VM.PlayCmd;
                else
                    item.Command = VM.DownCmd;
            }
        }
    }
}
