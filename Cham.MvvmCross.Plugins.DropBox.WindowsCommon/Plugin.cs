using Cham.MvvmCross.Plugins.DropBox;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cham.MvvmCross.Plugins.DropBox.WindowsCommon
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            
            Mvx.RegisterSingleton<IMvxDBDataStore>(() => new MvxDBDataStore());
        }
    }
}
