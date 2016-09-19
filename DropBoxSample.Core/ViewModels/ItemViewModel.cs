using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Xamarin.Plugins.DropBox;
using Xamarin.Plugins.DropBox.Abstractions;

namespace DropBoxSample.Core.ViewModels
{
    public class ItemViewModel : MvxViewModel
    {
        private IDbDataStore dataStore;
        private IDbTable<Item> _table;
        private readonly FirstViewModel Parent;

        public ItemViewModel(Item model, FirstViewModel parent, IDbDataStore dataStore)
        {
            Model = model;
            Parent = parent;
            this.dataStore = dataStore;
        }

        private IDbTable<Item> Table
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
                if (Parent.AutoSync) dataStore.Sync();
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    bool isSelected = Parent.SelectedItem?.Model.Id == Model.Id;
                    Table.Delete(Model);
                    Parent.Items.Remove(this);
                    if (isSelected && Parent.Items.Count > 0) Parent.SelectedItem = Parent.Items[0];
                });
            }
        }
    }
}