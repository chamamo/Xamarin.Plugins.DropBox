using Cham.MvvmCross.Plugins.DropBox;
using Cham.MvvmCross.Plugins.DropBox.Messages;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DropBoxSample.Core.ViewModels
{
    public class FirstViewModel 
		: MvxViewModel
    {
        private string DropboxSyncKey = "uhzc6l5g0he31la";
        private string DropboxSyncSecret = "eb961wew2q9y2yz";
        private IMvxDBDataStore _dataStore;

        public FirstViewModel()
        {
            _dataStore = Mvx.Resolve<IMvxDBDataStore>();
            Items = new ObservableCollection<ItemViewModel>();
            var messenger = Mvx.Resolve<IMvxMessenger>();
            messenger.Subscribe<DrbxReceivedMessage<Item>>(m =>
            {
                var itemVM = Items.SingleOrDefault(vm => vm.Model.Id == m.Record.Id);
                if (m.Record.IsDeleted)
                {
                    if (itemVM != null)
                    {
                        Items.Remove(itemVM);
                    }
                }
                else
                {
                    if (itemVM != null)
                    {
                        var model = itemVM.Model;
                        m.Record.Populate<Item>(ref model);
                        itemVM.Model = model;
                        itemVM.RaiseAllPropertiesChanged();
                    }
                    else
                    {
                        var model = new Item();
                        m.Record.Populate<Item>(ref model);
                        Items.Add(new ItemViewModel(model, this, _dataStore));
                        if (Items.Count == 1 && SelectedItem == null) SelectedItem = Items[0];
                    }
                }
            });
        }

        IMvxDBTable<Item> _table;
        private IMvxDBTable<Item> Table
        {
            get
            {
                if (_table == null) _table = _dataStore.GetTable<Item>();
                return _table;
            }
        }

        public ICommand ConnectCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _dataStore.Init(DropboxSyncKey, DropboxSyncSecret);
                    RaisePropertyChanged(() => Online);
                });
            }
        }

        public bool Online
        {
            get
            {
                return _dataStore.HasLinkedAccount;
            }
        }

        public IMvxCommand SyncCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    RefreshData();
                });
            }
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

        public IMvxCommand AddCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    var item = new Item() { Id = Guid.NewGuid().ToString(), Value = "new" };
                    Table.GetOrInsert(item);
                    var itemVM = new ItemViewModel(item, this, _dataStore);
                    Items.Add(itemVM);
                    RaisePropertyChanged(() => Items);
                    SelectedItem = itemVM;
                });
            }
        }

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
    }

    public class ItemViewModel : MvxViewModel
    {
        private IMvxDBDataStore dataStore;
        private IMvxDBTable<Item> _table;
        private readonly FirstViewModel Parent;

        public ItemViewModel(Item model, FirstViewModel parent, IMvxDBDataStore dataStore)
        {
            Model = model;
            Parent = parent;
            this.dataStore = dataStore;
        }

        private IMvxDBTable<Item> Table
        {
            get
            {
                if (_table == null) _table = dataStore.GetTable<Item>();
                return _table;
            }
        }

        public Item Model { get; set; }

        private bool _checked;
        public bool Checked
        {
            get { return _checked; }
            set
            {
                if (_checked == value) return;
                _checked = value;                
                RaisePropertyChanged(() => Checked);               
            }
        }

        public string Value
        {
            get { return Model.Value; }
            set
            {
                if (Model.Value == value) return;
                Model.Value = value;
                RaisePropertyChanged(() => Value);
                var record = Table.Get(Model.Id);
                record["Value"] = value;
                dataStore.Sync();
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new MvxCommand(() =>
                    {
                        Table.Delete(Model);
                        Parent.Items.Remove(this);
                    });
            }
        }
    }

    public class Item : IMvxDBEntity
    {
        [MvxDBKey]
        public string Id { get; set; }

        public string Value { get; set; }

        [MvxDBIgnore]
        public bool Changed { get; set; }
    }
}
