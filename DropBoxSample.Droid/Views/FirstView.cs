using Android.App;
using Android.OS;
using DropBoxSample.Core.ViewModels;
using MvvmCross.Droid.Views;
using Xamarin.Plugins.DropBox;

namespace DropBoxSample.Droid.Views
{
    [Activity(Label = "View for FirstViewModel")]
    public class FirstView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            var act = Xamarin.Plugins.DropBox.Helper.GetCurrentActivity();
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.FirstView);
        }

        protected override void OnResume()
        {
            base.OnResume();
            var vm = ViewModel as FirstViewModel;
            vm.Refresh();
        }
    }
}