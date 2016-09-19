using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Xamarin.Plugins.DropBox;
using Xamarin.Plugins.DropBox.Abstractions;

namespace DropBoxSample.Core.ViewModels
{
    public class FirstViewModel 
		: MvxViewModel
    {
        private const string DropboxSyncKey = "k9hhmzy4j60y8hm";
        private const string DropboxSyncSecret = "gbntl1z7d5fdu5j";
        private readonly IDbDataStore _dataStore;

        public FirstViewModel()
        {
            Items = new ObservableCollection<ItemViewModel>();
            _dataStore = CrossDbDataStore.Current;
            _dataStore.InitMapping(typeof(Item).GetTypeInfo().Assembly);
            _dataStore.DbRecordChanged += DbRecordChanged;
        }
        
        private IDbTable<Item> Table => _dataStore.GetTable<Item>();
        

        public ICommand ConnectCommand => new MvxCommand(() =>
        {
            _dataStore.Init(DropboxSyncKey, DropboxSyncSecret);
            RaisePropertyChanged(() => Online);
            RefreshData();
        });

        public bool Online => _dataStore.HasLinkedAccount;


        private bool _autoSync;
        public bool AutoSync
        {
            get { return _autoSync; }
            set
            {
                if (_autoSync == value) return;
                _autoSync = value;
                RaisePropertyChanged(() => AutoSync);
            }
        }

        public IMvxCommand SyncCommand => new MvxCommand(RefreshData);

        public IMvxCommand AddCommand => new MvxCommand(() =>
        {
            var item = new Item() {Id = Guid.NewGuid().ToString(), Value = "new"};
            Table.GetOrInsert(item, AutoSync);
            var itemVM = new ItemViewModel(item, this, _dataStore);
            Items.Add(itemVM);
            RaisePropertyChanged(() => Items);
            SelectedItem = itemVM;
        });

        private ObservableCollection<ItemViewModel> _items;
        public ObservableCollection<ItemViewModel> Items
        {
            get { return _items; }
            set
            {
                if (_items == value) return;
                _items = value;
                RaisePropertyChanged(() => Items);
            }
        }

        private ItemViewModel _selectedItem;
        public ItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                if (_selectedItem != null) _selectedItem.Checked = false;
                _selectedItem = value;
                _selectedItem.Checked = true;
                RaisePropertyChanged(() => SelectedItem);
                SelectedItem.RaiseAllPropertiesChanged();
            }
        }

        public void Refresh()
        {
            RaisePropertyChanged(() => Online);
            RefreshData();
        }

        private void RefreshData()
        {
            if (_dataStore.HasLinkedAccount)
            {
                Items.Clear();
                var results = Table.Query();
                if (results != null)
                {
                    foreach (var result in results)
                    {
                        Item item = new Item();
                        result.Populate<Item>(ref item);
                        Items.Add(new ItemViewModel(item, this, _dataStore));
                    }
                }
                _dataStore.Sync();
            }
        }

        private void DbRecordChanged(object sender, DbRecordChangedEventArgs e)
        {
            var itemVm = Items.SingleOrDefault(vm => vm.Model.Id == e.DbRecord.Id);
            if (e.DbRecord.IsDeleted)
            {
                if (itemVm != null)
                {
                    Items.Remove(itemVm);
                }
            }
            else
            {
                if (itemVm != null)
                {
                    var model = itemVm.Model;
                    e.DbRecord.Populate<Item>(ref model);
                    itemVm.Model = model;
                    itemVm.RaiseAllPropertiesChanged();
                    if (SelectedItem != null && SelectedItem.Model.Id == model.Id) SelectedItem.RaiseAllPropertiesChanged();
                }
                else
                {
                    var model = new Item();
                    e.DbRecord.Populate<Item>(ref model);
                    Items.Add(new ItemViewModel(model, this, _dataStore));
                    if (Items.Count == 1 && SelectedItem == null) SelectedItem = Items[0];
                }
            }
        }
    }
}
