using System;
using System.Collections.Generic;
using System.Text;
using DropBoxSync.iOS;
using UIKit;
using Xamarin.Plugins.DropBox.Abstractions;

namespace Xamarin.Plugins.DropBox
{
    class DbDataStore : DbDataStoreBase, IDbDataStore
    {
        public event EventHandler<DbRecordChangedEventArgs> DbRecordChanged;
        private string _dropboxSyncKey;
        private string _dropboxSyncSecret;

        private DBAccountManager Account { get; set; }

        public bool HasLinkedAccount => Account?.LinkedAccount != null;

        private DBDatastore DropboxDatastore { get; set; }

        public long Size => (long)DropboxDatastore.Size;

        public long UnsyncedChangesSize => (long)DropboxDatastore.UnsyncedChangesSize;

        public long RecordCount => (long)DropboxDatastore.RecordCount;

        public void Unlink()
        {
            Account?.Unlink();
        }

        public IDbTable<T> GetTable<T>(string tableName) where T : IDbEntity
        {
            throw new NotImplementedException();
        }

        public IDbTable<T> GetTable<T>() where T : IDbEntity
        {
            throw new NotImplementedException();
        }

        public void Init(string appKey, string appSecret)
        {
            _dropboxSyncKey = appKey;
            _dropboxSyncSecret = appSecret;

            var manager = new DBAccountManager(_dropboxSyncKey, _dropboxSyncSecret);
            DBAccountManager.SharedManager = manager;
            var account = DBAccountManager.SharedManager.LinkedAccount;
            if (account != null)
            {
                StartApp(account);
            }
            else
            {
                var window = UIApplication.SharedApplication.KeyWindow;
                manager.LinkFromController(window.RootViewController);
            }

            throw new NotImplementedException();
        }

        void StartApp(DBAccount account = null)
        {
            InitializeDropbox(account);
            Sync();
        }

        private void InitializeDropbox(DBAccount account)
        {
            if (DropboxDatastore == null || !DropboxDatastore.Open || DropboxDatastore.Manager.IsShutDown)
            {
                DBError error;
                DropboxDatastore = DBDatastore.OpenDefaultStore(account ?? Account.LinkedAccount, out error);
                //Todo DropboxDatastore.DatastoreChanged += HandleStoreChange;
            }
        }

        public void Sync()
        {
            DBError error;
            var changes = DropboxDatastore?.Sync(out error);
            if (changes != null && changes.Count > 0)
            {
                foreach (var change in changes)
                {
                    var map = DbMapping.Get(change.Key.ToString());
                    if (map == null) continue;
                    foreach (DBRecord record in change.Value)
                    {
                        DbRecordChanged?.Invoke(this, new DbRecordChangedEventArgs(record.ToDbRecord()));
                    }
                }
            }
        }

        public void Delete()
        {
            if (DropboxDatastore != null && DropboxDatastore.Open)
            {
                DropboxDatastore.Close();
                DBError error;
                DropboxDatastore.Manager.DeleteDatastore("default", out error);
            }
        }
    }
}
