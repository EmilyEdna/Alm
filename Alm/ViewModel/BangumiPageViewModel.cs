using Alm.Utils;
using Alm.ViewModel.Base;
using AlmCore.Scrapy;
using AlmCore.Scrapy.ImomoeModel;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using XExten.XCore;

namespace Alm.ViewModel
{
    public class BangumiPageViewModel : ViewModelBase
    {
        public BangumiPageViewModel()
        {
            IocManager.SetCache(nameof(BangumiPageViewModel), this);
            Type = "类型";
        }

        private string _Type;
        public string Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
                OnPropertyChanged("Type");
            }
        }

        private BangumiRoot _Root;
        public BangumiRoot Root
        {
            get
            {
                return _Root;
            }
            set
            {
                _Root = value;
                OnPropertyChanged("Root");
            }
        }

        #region Commands
        public Commands<string> SelectCmd => new Commands<string>((str) =>
        {
            if (str.Equals("Name")) Type = "名称";
            else Type = "年份";
        }, null);

        public Commands<string> SearchCmd => new Commands<string>((obj) =>
        {
            Root = null;
            if (Type.Equals("类型")) { Growl.Warning("请选择类型"); return; }
            if (obj.IsNullOrEmpty()) { Growl.Warning("查询条件不能未空"); return; }
            if (Type.Equals("名称"))
            {
                Root = Imomoe.GetBangumi(obj);
            }
            else {
                int.TryParse(obj, out int Defualt);
                if (Defualt == 0)
                {
                    Growl.Warning("请输入数字");
                    return;
                }
                Root = Imomoe.GetBangumi(Defualt);
            }

        }, null);
        #endregion
    }
}
