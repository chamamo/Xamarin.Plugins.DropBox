using System;
using DropboxSync.Android;
using Xamarin.Plugins.DropBox.Abstractions;

namespace Xamarin.Plugins.DropBox
{
    public class DbDataStore : DbDataStoreBase, IDbDataStore
    {
        private string _dropboxSyncKey;
        private string _dropboxSyncSecret;

        private DBAccountManager Account { get; set; }

        public bool HasLinkedAccount => Account != null && Account.HasLinkedAccount;

        private DBDatastore DropboxDatastore { get; set; }

        public long Size => DropboxDatastore.Size;

        public long UnsyncedChangesSize => DropboxDatastore.UnsyncedChangesSize;

        public long RecordCount => DropboxDatastore.RecordCount;

        public void Sync()
        {
            var changes = DropboxDatastore?.Sync();
            if (changes != null && changes.Count > 0)
            {
                foreach (var change in changes)
                {
                    var map = DbMapping.Get(change.Key);
                    if (map == null) continue;
                    foreach (var record in change.Value)
                    {
                        DbRecordChanged?.Invoke(this, new DbRecordChangedEventArgs(record.ToDbRecord()));
                    }
                }
            }
        }

        public event EventHandler<DbRecordChangedEventArgs> DbRecordChanged;

        public void Delete()
        {
            if (DropboxDatastore != null && DropboxDatastore.IsOpen)
            {
                DropboxDatastore.Close();
                DropboxDatastore.Manager.DeleteDatastore("default");
            }
        }

        public void Unlink()
        {
            Account?.Unlink();
        }

        public void Init(string appKey, string appSecret)
        {
            _dropboxSyncKey = appKey;
            _dropboxSyncSecret = appSecret;
            var activity = Helper.GetCurrentActivity();
            Account = DBAccountManager.GetInstance(activity.ApplicationContext, _dropboxSyncKey, _dropboxSyncSecret);
            Account.LinkedAccountChanged -= LinkedAccountChanged;
            Account.LinkedAccountChanged += LinkedAccountChanged;
            // TODO: Restart auth flow.
            if (!Account.HasLinkedAccount)
            {
                Account.StartLink(activity, (int)RequestCode.LinkToDropboxRequest);
            }
            else
            {
                StartApp();
            }
        }

        private void LinkedAccountChanged(object sender, DBAccountManager.AccountChangedEventArgs e)
        {
            var activity = Helper.GetCurrentActivity();
            if (!e.P1.IsLinked)
            {
                Account.StartLink(activity, (int)RequestCode.LinkToDropboxRequest);
                return;
            }
            Account = e.P0;
            StartApp(e.P1);
        }


        private void InitializeDropbox(DBAccount account)
        {
            //Log("InitializeDropbox");
            if (DropboxDatastore == null || !DropboxDatastore.IsOpen || DropboxDatastore.Manager.IsShutDown)
            {
                DropboxDatastore = DBDatastore.OpenDefault(account ?? Account.LinkedAccount);
                DropboxDatastore.DatastoreChanged += HandleStoreChange;
            }
        }

        void HandleStoreChange(object sender, DBDatastore.SyncStatusEventArgs e)
        {
            if (e.P0.SyncStatus.HasIncoming)
            {
                if (!Account.HasLinkedAccount)
                {
                    //Log("InitializeDropbox", "Account no longer linked, so abandoning.");
                    DropboxDatastore.DatastoreChanged -= HandleStoreChange;
                }
                Console.WriteLine("Datastore needs to be re-synced.");
                Sync();
            }
        }

        void StartApp(DBAccount account = null)
        {
            InitializeDropbox(account);
            Sync();
        }
        
        bool VerifyStore()
        {
            if (!DropboxDatastore.IsOpen)
            {
                //Log("VerifyStore", "Datastore is NOT open.");
                return false;
            }
            if (DropboxDatastore.Manager.IsShutDown)
            {
                //Log("VerifyStore", "Manager is shutdown.");
                return false;
            }
            if (!Account.HasLinkedAccount)
            {
                //Log("VerifyStore", "Account was unlinked while we weren't watching.");
                return false;
            }
            return true;
        }

        void RestartAuthFlow()
        {
            if (Account.HasLinkedAccount)
                Account.Unlink();
            else
                Account.StartLink(Helper.GetCurrentActivity(), (int)RequestCode.LinkToDropboxRequest);
        }

        public IDbTable<T> GetTable<T>(string tableName) where T : IDbEntity
        {
            if (DropboxDatastore == null) return null;
            return new DbTable<T>(DropboxDatastore.GetTable(tableName), this);
        }

        public IDbTable<T> GetTable<T>() where T : IDbEntity
        {
            return GetTable<T>(typeof(T).Name);
        }
    }
}