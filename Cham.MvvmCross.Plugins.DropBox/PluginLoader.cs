using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cham.MvvmCross.Plugins.DropBox
{
    public class PluginLoader : IMvxPluginLoader
    {
        public static readonly PluginLoader Instance = new PluginLoader();

        public void EnsureLoaded()
        {
            var manager = Mvx.Resolve<IMvxPluginManager>();
            manager.EnsurePlatformAdaptionLoaded<PluginLoader>();
        }

        public void InitMapping(Assembly assembly)
        {
            var query = assembly.GetTypes().Where(t => typeof(IMvxDBEntity).IsAssignableFrom(t));
            foreach (var type in query)
            {
                MvxDBMapping.Add(type);
            }
           
        }
    }
}
