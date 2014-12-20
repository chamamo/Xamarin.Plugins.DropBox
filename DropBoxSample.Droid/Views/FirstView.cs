using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;
using DropBoxSample.Core.ViewModels;

namespace DropBoxSample.Droid.Views
{
    [Activity(Label = "View for FirstViewModel")]
    public class FirstView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
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