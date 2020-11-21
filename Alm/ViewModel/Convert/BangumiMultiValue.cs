using Alm.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Alm.ViewModel.Convert
{
    public class BangumiMultiValue: BaseConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new Dictionary<string, string>() { { values[0].ToString(), values[1].ToString() } };
        }
    }
}
