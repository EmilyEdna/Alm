using Alm.Controls;
using Alm.Utils;
using Alm.ViewModel.Base;
using AlmCore.Scrapy;
using AlmCore.Scrapy.KonachanModel;
using AlmCore.SQLModel.Konachans;
using AlmCore.SQLService;
using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using XExten.XCore;

namespace Alm.ViewModel
{
    public class KonachanPageViewModel : ViewModelBase
    {
        public KonachanPageViewModel()
        {
            IocManager.SetCache(nameof(KonachanPageViewModel), this);
            Type = "类型";
            PageIndex = 1;
            Root = Konachan.GetImage();
            Tags = KonachanLogic.Logic.GetUserTags();
        }

        #region Property
        private string _Type;
        public string Type
        {
            get { return _Type; }
            set { _Type = value; OnPropertyChanged("Type"); }
        }
        private string _KeyWord;
        public string KeyWord
        {
            get { return _KeyWord; }
            set { _KeyWord = value; OnPropertyChanged("KeyWord"); }
        }
        private int _PageIndex;
        public int PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; OnPropertyChanged("PageIndex"); }
        }
        private ImageRoot _Root;
        public ImageRoot Root
        {
            get { return _Root; }
            set { _Root = value; OnPropertyChanged("Root"); }
        }
        private List<UserTags> _Tags;
        public List<UserTags> Tags
        {
            get { return _Tags; }
            set { _Tags = value; OnPropertyChanged("Tags"); }
        }
        #endregion

        #region Commands
        public Commands<FunctionEventArgs<int>> PageUpdatedCmd => new Commands<FunctionEventArgs<int>>((obj) =>
        {
            PageIndex = obj.Info;
            Root = Konachan.GetImage(PageIndex, null, ex => Growl.Error(ex.Message));
        }, null);

        public Commands<Dictionary<CollectBtn, ImageElements>> CollectCmd => new Commands<Dictionary<CollectBtn, ImageElements>>((obj) =>
        {
            ResourceDictionary dictionary = new ResourceDictionary()
            {
                Source = new Uri(@"/Alm;component/Style/Geometry.xaml", UriKind.Relative)
            };
            ImageElements Elements = obj.Values.FirstOrDefault();
            CollectBtn Btn = obj.Keys.FirstOrDefault();
            if (KonachanLogic.Logic.HasCollect(Elements.Id))
            {
                KonachanLogic.Logic.AddCollect(Elements);
                Btn.Icon = Geometry.Parse(dictionary["YesStarIcon"].ToString());
            }
            else
            {
                KonachanLogic.Logic.RemoveCollect(Elements.Id);
                Btn.Icon = Geometry.Parse(dictionary["NoStarIcon"].ToString());
            }

        }, null);

        public Commands<ImageElements> DownCmd => new Commands<ImageElements>((obj) =>
        {
            if (KonachanLogic.Logic.CheckRecord(obj.Id))
            {
                KonachanLogic.Logic.AddDownRecord(new DownRecord
                {
                    DownTime = DateTime.Now,
                    FileURL = obj.FileURL.ToLzStringEnc(),
                    Id = obj.Id,
                    Name = obj.Tag,
                    FileSize = obj.FileSizeMB + "MB"
                });
                Growl.Info("已加入到下载列表");
            }
            else Growl.Info("已经在下载列表了");
        }, null);

        public Commands<object> SearchCmd => new Commands<object>((obj) =>
        {
        }, null);
        #endregion
    }
}
