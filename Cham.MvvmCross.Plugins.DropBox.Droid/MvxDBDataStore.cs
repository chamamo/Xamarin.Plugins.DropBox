using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;
using Cirrious.MvvmCross.Plugins.Messenger;
using DropboxSync.Android;
using Cham.MvvmCross.Plugins.DropBox.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cham.MvvmCross.Plugins.DropBox.Droid
{
    enum RequestCode
    {
        LinkToDropboxRequest
    }

    public class MvxDBDataStore : MvxDBDataStoreBase, IMvxDBDataStore
    {
        private string DropboxSyncKey;
        private string DropboxSyncSecret;

        private DBAccountManager Account { get; set; }

        public override bool HasLinkedAccount { get { return Account != null ? Account.HasLinkedAccount : false; } }

        private DBDatastore DropboxDatastore { get; set; }

        public override void Sync()
        {
            if (DropboxDatastore == null) return;
            var changes = DropboxDatastore.Sync();
            if (changes != null && changes.Count > 0)
            {
                var messageType = typeof(DrbxReceivedMessage<>);
                var messenger = Mvx.Resolve<IMvxMessenger>();
                foreach (var change in changes)
                {
                    var map = MvxDBMapping.Get(change.Key);
                    if (map == null) continue;
                    Type[] typeArgs = { map.Type };
                    var makeme = messageType.MakeGenericType(typeArgs);
                    foreach (var record in change.Value)
                    {
                        var m = Activator.CreateInstance(makeme, this, record.ToMvxDBRecord()) as MvxMessage;
                        messenger.Publish(m, makeme);
                    }
                }
            }
        }

        public override void Delete()
        {
            if (DropboxDatastore != null && DropboxDatastore.IsOpen)
            {
                DropboxDatastore.Close();
            }
            DropboxDatastore.Manager.DeleteDatastore("default");
        }

        public override void Unlink()
        {
            if (Account != null) Account.Unlink();
        }

        public override void Init(string appKey, string appSecret)
        {
            DropboxSyncKey = appKey;
            DropboxSyncSecret = appSecret;
            var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            Account = DBAccountManager.GetInstance(activity.ApplicationContext, DropboxSyncKey, DropboxSyncSecret);
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
            var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
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
                Account.StartLink(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity, (int)RequestCode.LinkToDropboxRequest);
        }

        public override IMvxDBTable<T> GetTable<T>(string tableName)
        {
            return new MvxDBTable<T>(DropboxDatastore.GetTable(tableName), this);
        }

        public override IMvxDBTable<T> GetTable<T>()
        {
            return GetTable<T>(typeof(T).Name);
        }
    }
}
