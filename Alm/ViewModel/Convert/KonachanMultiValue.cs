using Alm.Controls;
using Alm.ViewModel.Base;
using AlmCore.Scrapy.KonachanModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Alm.ViewModel.Convert
{
    public class KonachanMultiValue : BaseConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Dictionary<CollectBtn, ImageElements> Map = new Dictionary<CollectBtn, ImageElements>
            {
                { (values[1] as CollectBtn),(values[0] as ImageElements)  }
            };
            return Map;
        }
    }
}
