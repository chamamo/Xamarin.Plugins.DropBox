using Cham.MvvmCross.Plugins.DropBox;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;

namespace DropBoxSample.Core.ViewModels
{
    public class FirstViewModel 
		: MvxViewModel
    {
        private readonly IMvxDBDataStore DataStore;
        private string DropboxSyncKey = "uhzc6l5g0he31la";
        private string DropboxSyncSecret = "eb961wew2q9y2yz";

        public FirstViewModel()
        {
            DataStore = Mvx.Resolve<IMvxDBDataStore>();
        }

		private string _hello = "Hello MvvmCross";
        public string Hello
		{ 
			get { return _hello; }
			set { _hello = value; RaisePropertyChanged(() => Hello); }
		}

        public IMvxCommand SyncCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    DataStore.Init(DropboxSyncKey, DropboxSyncSecret);
                    DataStore.Sync();
                    var table = DataStore.GetTable<Test>();
                    table.AddOrUpdate(new Test() { Name = "Name", Value = "Value" }, true);
                    //Mvx.Resolve<IMvxMessenger>().Publish<DrbxUpMessage<Test>>(new DrbxUpMessage<Test>(new Test() { Name = "Name", Value = "Value" }, DrbxUpMessageType.Added, "Name"));
                });
            }
        }
    }

    public class Test : IMvxDBEntity
    {
        [MvxDBKey]
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
