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
        /// 全局
        /// </summary>
        public Commands<FunctionEventArgs<object>> SelectCmd => new Commands<FunctionEventArgs<object>>((obj) =>
        {
            Growl.Info((obj.Info as SideMenuItem)?.Header.ToString());
        }, null);

        /// <summary>
        /// 局部
        /// </summary>
        public Commands<string> SelectItem => new Commands<string>((obj) => 
        {
        
        }, null);
    }
}
