using System.Reflection;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;
using Xamarin.Plugins.DropBox;

namespace DropBoxSample.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
				
            RegisterAppStart<ViewModels.FirstViewModel>();
        }
    }
}