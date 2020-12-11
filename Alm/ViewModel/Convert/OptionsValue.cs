using Alm.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using XExten.XCore;

namespace Alm.ViewModel.Convert
{
    public class OptionsValue: BaseConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().ToMD5();
        }
    }
}
