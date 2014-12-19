using Cham.MvvmCross.Plugins.DropBox;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace DropBoxSample.Core.ViewModels
{
    public class FirstViewModel 
		: MvxViewModel
    {

        public FirstViewModel()
        {
            Collection1 = new CollectionViewModel();
            Collection2 = new CollectionViewModel();
        }

        private CollectionViewModel _collection1;
        public CollectionViewModel Collection1
        {
            get { return _collection1; }
            set
            {
                if (_collection1 == value) return;
                _collection1 = value;
                RaisePropertyChanged(() => Collection1);
            }
        }

        private CollectionViewModel _collection2;
        public CollectionViewModel Collection2
        {
            get { return _collection2; }
            set
            {
                if (_collection2 == value) return;
                _collection2 = value;
                RaisePropertyChanged(() => Collection2);
            }
        }
       
    }

    public class CollectionViewModel : MvxViewModel
    {
        
        private string DropboxSyncKey = "uhzc6l5g0he31la";
        private string DropboxSyncSecret = "eb961wew2q9y2yz";

        public CollectionViewModel()
        {
            Items = new ObservableCollection<ItemViewModel>();
        }

        private IMvxDBDataStore _dataStore;
        private IMvxDBDataStore DataStore
        {
            get
            {
                if (_dataStore == null)
                {
                    _dataStore = Mvx.Resolve<IMvxDBDataStore>();
                    _dataStore.Init(DropboxSyncKey, DropboxSyncSecret);
                }
                return _dataStore;
            }
        }

        public IMvxCommand SyncCommand
        {
            get
            {
                return new MvxCommand(() =>
                {                    
                    DataStore.Sync();
                    var table = DataStore.GetTable<Item>();
                    foreach (var item in Items)
                    {
                        table.AddOrUpdate(item.Model, false);
                    }
                    DataStore.Sync();
                });
            }
        }

        public IMvxCommand AddCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    var item = new Item() { Id = Guid.NewGuid().ToString(), Value = string.Empty };
                    var itemVM = new ItemViewModel(item);
                    Items.Add(itemVM);
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
            }
        }
    }

    public class ItemViewModel : MvxViewModel
    {
        public ItemViewModel(Item model)
        {
            Model = model;
        }

        public Item Model { get; private set; }

        private bool _checked;
        public bool Checked
        {
            get { return _checked; }
            set
            {
                if (_checked == value) return;
                Checked = value;
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
            }
        }
    }

    public class Item : IMvxDBEntity
    {
        [MvxDBKey]
        public string Id { get; set; }

        public string Value { get; set; }
    }
}
