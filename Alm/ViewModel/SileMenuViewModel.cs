using Alm.Pages;
using Alm.Utils;
using Alm.Utils.Enums;
using Alm.ViewModel.Base;
using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alm.ViewModel
{
    public class SileMenuViewModel : ViewModelBase
    {
        /// <summary>
        /// 局部
        /// </summary>
        public Commands<string> SelectItem => new Commands<string>((str) =>
        {
            MainWindowViewModel main = IocManager.GetCache<MainWindowViewModel>(nameof(MainWindowViewModel));
            FunctionEnums function = Enum.Parse<FunctionEnums>(str);
            switch (function)
            {
                case FunctionEnums.Search:
                    break;
                case FunctionEnums.Konachan:
                    main.CurrentPage = new KonachanPage();
                    break;
                case FunctionEnums.Bangumi:
                    main.CurrentPage = new BangumiPage();
                    break;
                case FunctionEnums.PlayHistory:
                    main.CurrentPage = new PlayHistoryPage();
                    break;
                case FunctionEnums.ImageCollect:
                    main.CurrentPage = new KonachanCollectPage();
                    break;
                case FunctionEnums.DownRecord:
                    break;
                case FunctionEnums.Author:
                    break;
                default:
                    break;
            }
        }, null);
    }
}
