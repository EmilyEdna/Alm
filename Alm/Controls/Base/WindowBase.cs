using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace Alm.Controls.Base
{
    public class WindowBase : Window
    {
        #region Property
        /// <summary>
        /// 窗体阴影大小
        /// </summary>
        [Description("窗体阴影大小")]
        public double WindowShadowSize
        {
            get { return (double)GetValue(WindowShadowSizeProperty); }
            set { SetValue(WindowShadowSizeProperty, value); }
        }
        public static readonly DependencyProperty WindowShadowSizeProperty =
            DependencyProperty.Register("WindowShadowSize", typeof(double), typeof(WindowBase), new PropertyMetadata(10.0));

        /// <summary>
        /// 窗体阴影颜色
        /// </summary>
        [Description("窗体阴影颜色")]
        public Color WindowShadowColor
        {
            get { return (Color)GetValue(WindowShadowColorProperty); }
            set { SetValue(WindowShadowColorProperty, value); }
        }
        public static readonly DependencyProperty WindowShadowColorProperty =
            DependencyProperty.Register("WindowShadowColor", typeof(Color), typeof(WindowBase), new PropertyMetadata(Color.FromArgb(255, 200, 200, 200)));

        /// <summary>
        /// 窗体阴影透明度
        /// </summary>
        [Description("窗体阴影透明度")]
        public double WindowShadowOpacity
        {
            get { return (double)GetValue(WindowShadowOpacityProperty); }
            set { SetValue(WindowShadowOpacityProperty, value); }
        }
        public static readonly DependencyProperty WindowShadowOpacityProperty =
            DependencyProperty.Register("WindowShadowOpacity", typeof(double), typeof(WindowBase), new PropertyMetadata(1.0));
        #endregion

        #region WinDLL
        [DllImport("user32.dll")]

        public static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        #endregion

        #region  Event
        IntPtr Prt = IntPtr.Zero;
        public void InitEvent()
        {
            SourceInitialized += WindowBase_SourceInitialized;
            MouseLeftButtonDown += WindowBase_MouseLeftButtonDown;
        }
        private void WindowBase_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Grid || e.OriginalSource is Window || e.OriginalSource is Border)
            {
                SendMessage(Prt, 0x00A1, (IntPtr)2, IntPtr.Zero);
                return;
            }
        }

        private void WindowBase_SourceInitialized(object sender, EventArgs e)
        {
            Prt = new WindowInteropHelper(this).Handle;
        }
        #endregion
    }
}
