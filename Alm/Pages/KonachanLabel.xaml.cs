using Alm.Pages.Confirms;
using Alm.Utils;
using Alm.ViewModel;
using Alm.ViewModel.Confirms;
using AlmCore.SQLModel.Konachans;
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
using XExten.XCore;

namespace Alm.Pages
{
    /// <summary>
    /// KonachanLabel.xaml 的交互逻辑
    /// </summary>
    public partial class KonachanLabel 
    {
        public KonachanLabel()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Confirm confirm = new Confirm()
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Owner = this
            };
            if (confirm.ShowDialog().Value)
            {
                var result = ViewModelLocator.Instance.ConfirmVM.Result;
                if (!result.IsNullOrEmpty())
                {
                    KonachanLogic.Logic.AddUserTags(new UserTags
                    {
                        AddTime = DateTime.Now,
                        DiyName = result,
                        DiyValue = (sender as Button).CommandParameter.ToString()
                    });
                }
            }
        }
    }
}
