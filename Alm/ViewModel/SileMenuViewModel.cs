﻿using Alm.Pages;
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
            FunctionEnum function = Enum.Parse<FunctionEnum>(str);
            switch (function)
            {
                case FunctionEnum.Search:
                    break;
                case FunctionEnum.Konachan:
                    main.CurrentPage = new KonachanPage();
                    break;
                case FunctionEnum.Bangumi:
                    main.CurrentPage = new BangumiPage();
                    break;
                case FunctionEnum.PlayHistory:
                    main.CurrentPage = new PlayHistoryPage();
                    break;
                case FunctionEnum.ImageCollect:
                    main.CurrentPage = new KonachanCollectPage();
                    break;
                case FunctionEnum.DownRecord:
                    main.CurrentPage = new DownRecordPage();
                    break;
                case FunctionEnum.Author:
                    break;
                default:
                    break;
            }
        }, null);
    }
}
