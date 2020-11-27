using Alm.Utils.Enums;
using Alm.ViewModel.Base;
using AlmCore.SQLModel.Konachans;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Alm.ViewModel.Convert
{
    public class DownloadMultiValue : BaseConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            DownloadEnum download = (DownloadEnum)System.Convert.ToInt32(values[0]);
            return new Dictionary<DownloadEnum, DownRecord> { { download, (DownRecord)values[1] } };
        }
    }
}
