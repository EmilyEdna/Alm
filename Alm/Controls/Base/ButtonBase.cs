using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Alm.Controls.Base
{
    public class ButtonBase : Button
    {
        #region Property
        /// <summary>
        /// 图标
        /// </summary>
        public Geometry Icon
        {
            get { return (Geometry)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
          DependencyProperty.Register("Icon", typeof(Geometry), typeof(ButtonBase), new PropertyMetadata(null));

        /// <summary>
        /// 窗体系统按钮图标高度
        /// </summary>
        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }
        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register("IconHeight", typeof(double), typeof(ButtonBase), new PropertyMetadata(1.0));

        /// <summary>
        /// 窗体系统按钮图标宽度
        /// </summary>
        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register("IconWidth", typeof(double), typeof(ButtonBase), new PropertyMetadata(1.0));

        /// <summary>
        /// 窗体系统按钮大小
        /// </summary>
        public double ButtonSize
        {
            get { return (double)GetValue(ButtonSizeProperty); }
            set { SetValue(ButtonSizeProperty, value); }
        }
        public static readonly DependencyProperty ButtonSizeProperty =
            DependencyProperty.Register("ButtonSize", typeof(double), typeof(ButtonBase), new PropertyMetadata(30.0));

        /// <summary>
        /// 窗体系统按钮鼠标悬浮背景颜色
        /// </summary>
        public SolidColorBrush ButtonHoverColor
        {
            get { return (SolidColorBrush)GetValue(ButtonHoverColorProperty); }
            set { SetValue(ButtonHoverColorProperty, value); }
        }
        public static readonly DependencyProperty ButtonHoverColorProperty =
            DependencyProperty.Register("ButtonHoverColor", typeof(SolidColorBrush), typeof(ButtonBase), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(50, 50, 50, 50))));

        /// <summary>
        /// 窗体系统按钮颜色
        /// </summary>
        public SolidColorBrush ButtonForeground
        {
            get { return (SolidColorBrush)GetValue(ButtonForegroundProperty); }
            set { SetValue(ButtonForegroundProperty, value); }
        }
        public static readonly DependencyProperty ButtonForegroundProperty =
            DependencyProperty.Register("ButtonForeground", typeof(SolidColorBrush), typeof(ButtonBase), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 255, 255, 255))));

        /// <summary>
        /// 窗体系统按钮鼠标悬按钮颜色
        /// </summary>
        public SolidColorBrush ButtonHoverForeground
        {
            get { return (SolidColorBrush)GetValue(ButtonHoverForegroundProperty); }
            set { SetValue(ButtonHoverForegroundProperty, value); }
        }
        public static readonly DependencyProperty ButtonHoverForegroundProperty =
            DependencyProperty.Register("ButtonHoverForeground", typeof(SolidColorBrush), typeof(ButtonBase), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 255, 255, 255))));
        #endregion
    }
}
