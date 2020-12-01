using Alm.Utils;
using Alm.ViewModel.Confirms;
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

namespace Alm.Pages.Confirms
{
    /// <summary>
    /// Confirm.xaml 的交互逻辑
    /// </summary>
    public partial class Confirm 
    {
        public Confirm()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (sender as Button);
            if (Convert.ToBoolean(btn.CommandParameter))
            {
                IocManager.GetCache<ConfirmViewModel>(nameof(ConfirmViewModel)).Result = DataResult.Text;
                DialogResult = true;
                Close();
            }
            else {
                DialogResult = false;
                Close();
            }
        }
    }
}
