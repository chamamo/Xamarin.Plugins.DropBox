using Cirrious.CrossCore.IoC;
using System.Reflection;

namespace DropBoxSample.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
				
            RegisterAppStart<ViewModels.FirstViewModel>();
        }
        public override void LoadPlugins(Cirrious.CrossCore.Plugins.IMvxPluginManager pluginManager)
        {
            base.LoadPlugins(pluginManager);
            Cham.MvvmCross.Plugins.DropBox.PluginLoader.Instance.EnsureLoaded();
            Cham.MvvmCross.Plugins.DropBox.PluginLoader.Instance.InitMapping(GetType().GetTypeInfo().Assembly);
        }
    }
}