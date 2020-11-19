using Alm.Controls.Base;
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

        private void AlmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (targetWindow == null)
            {
                targetWindow = Window.GetWindow(this);
            }

            AlmBtn btn = sender as AlmBtn;
            var Param = btn.CommandParameter.ToString();
            if (Param.Equals("Close"))
            {
                targetWindow.Close();
            }
            else
            {
                targetWindow.WindowState = WindowState.Minimized;
            }
        }
    }
}
