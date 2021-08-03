using Alm.ViewModel.Confirms;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Alm.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<SileMenuViewModel>();
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<AuthorPageViewModel>();
            SimpleIoc.Default.Register<BangumiPageViewModel>();
            SimpleIoc.Default.Register<CollectPageViewModel>();
            SimpleIoc.Default.Register<DeveloperPageViewModel>();
            SimpleIoc.Default.Register<DownRecordPageViewModel>();
            SimpleIoc.Default.Register<KonachanLabelViewModel>();
            SimpleIoc.Default.Register<KonachanPageViewModel>();
            SimpleIoc.Default.Register<OptionsPageViewModel>();
            SimpleIoc.Default.Register<PlayHistoryPageViewModel>();
            SimpleIoc.Default.Register<TVSeriesPageViewModel>();


            SimpleIoc.Default.Register<ConfirmViewModel>();
        }
        public static ViewModelLocator Instance => Application.Current.TryFindResource("Locator") as ViewModelLocator;
        public AuthorPageViewModel AuthorPageVM => SimpleIoc.Default.GetInstance<AuthorPageViewModel>();
        public TVSeriesPageViewModel TVSeriesPageVM => SimpleIoc.Default.GetInstance<TVSeriesPageViewModel>();
        public BangumiPageViewModel BangumiPageVM => SimpleIoc.Default.GetInstance<BangumiPageViewModel>();
        public CollectPageViewModel CollectPageVM => SimpleIoc.Default.GetInstance<CollectPageViewModel>();
        public DeveloperPageViewModel DeveloperPageVM => SimpleIoc.Default.GetInstance<DeveloperPageViewModel>();
        public DownRecordPageViewModel DownRecordPageVM => SimpleIoc.Default.GetInstance<DownRecordPageViewModel>();
        public KonachanLabelViewModel KonachanLabelPageVM => SimpleIoc.Default.GetInstance<KonachanLabelViewModel>();
        public KonachanPageViewModel KonachanPageVM => SimpleIoc.Default.GetInstance<KonachanPageViewModel>();
        public MainWindowViewModel MainWindowVM => SimpleIoc.Default.GetInstance<MainWindowViewModel>();
        public OptionsPageViewModel OptionsPageVM => SimpleIoc.Default.GetInstance<OptionsPageViewModel>();
        public PlayHistoryPageViewModel PlayHistoryPageVM => SimpleIoc.Default.GetInstance<PlayHistoryPageViewModel>();
        public SileMenuViewModel SileMenuViewVM => SimpleIoc.Default.GetInstance<SileMenuViewModel>();


        public ConfirmViewModel ConfirmVM => SimpleIoc.Default.GetInstance<ConfirmViewModel>();
    }
}
