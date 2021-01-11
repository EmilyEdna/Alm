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
            FunctionEnum function = Enum.Parse<FunctionEnum>(str);
            switch (function)
            {
                case FunctionEnum.Music:
                    main.CurrentPage = new MusicPage();
                    break;
                case FunctionEnum.Konachan:
                    KonachanPINWindow PIN = new KonachanPINWindow();
                    if (PIN.ShowDialog() == true)
                    {
                        Growl.Info("解锁码正确!");
                        main.CurrentPage = new KonachanPage();
                    }
                    else
                    {
                        Growl.Info("解锁码错误!");
                        main.CurrentPage = new AuthorPage();
                    }
                    break;
                case FunctionEnum.Movie:
                    main.CurrentPage = new MoviePage();
                    break;
                case FunctionEnum.TVSeries:
                    main.CurrentPage = new TVSeriesPage();
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
                case FunctionEnum.Options:
                    main.CurrentPage = new OptionsPage();
                    break;
                case FunctionEnum.DeveloperNote:
                    main.CurrentPage = new DeveloperPage();
                    break;
                case FunctionEnum.Author:
                    main.CurrentPage = new AuthorPage();
                    break;
                default:
                    break;
            }
        }, null);
    }
}
