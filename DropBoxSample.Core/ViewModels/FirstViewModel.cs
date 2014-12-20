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
                var itemVM = Items.SingleOrDefault(vm => vm.Model.Id == m.Id);
                if (m.IsDeleted)
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
                        m.Changes.Populate<Item>(ref model);
                        itemVM.Model = model;
                    }
                    else
                    {
                        var model = new Item();
                        m.Changes.Populate<Item>(ref model);
                        Items.Add(new ItemViewModel(model, this, _dataStore));
                        if (Items.Count == 1 && SelectedItem == null) SelectedItem = Items[0];
                    }
                }
            });
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
                    if (_dataStore.HasLinkedAccount)
                    {
                        _dataStore.Sync();
                        var table = _dataStore.GetTable<Item>();
                        foreach (var item in Items)
                        {
                            table.AddOrUpdate(item.Model, false);
                        }
                        _dataStore.Sync();
                    }
                });
            }
        }

        public IMvxCommand AddCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    var item = new Item() { Id = Guid.NewGuid().ToString(), Value = "new" };
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
                Table.AddOrUpdate(Model);
            }
        }

        public ICommand ItemSelectedCommand
        {
            get { return new MvxCommand(() => Parent.SelectedItem = this); }
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
