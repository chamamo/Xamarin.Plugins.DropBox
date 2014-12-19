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

        private DBDatastore DropboxDatastore { get; set; }

        public override void Sync()
        {
            var changes = DropboxDatastore.Sync();
            if (changes != null && changes.Count > 0)
            {
                var messageType = typeof(DrbxReceivedMessage<>);
                var messenger = Mvx.Resolve<IMvxMessenger>();
                foreach (var change in changes)
                {
                    var map = MvxDBMapping.Get(change.Key);
                    Type[] typeArgs = { map.Type };
                    var makeme = messageType.MakeGenericType(typeArgs);
                    foreach (var record in change.Value)
                    {

                        var m = (record.IsDeleted ? Activator.CreateInstance(makeme, true) : Activator.CreateInstance(makeme, record.ToDictionary(map))) as MvxMessage;
                        messenger.Publish(m, makeme);
                    }
                }
            }
        }


        public override void Init(string appKey, string appSecret)
        {
            DropboxSyncKey = appKey;
            DropboxSyncSecret = appSecret;
            var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            Account = DBAccountManager.GetInstance(activity.ApplicationContext, DropboxSyncKey, DropboxSyncSecret);
            Account.LinkedAccountChanged += (sender, e) =>
            {
                if (!e.P1.IsLinked)
                {
                    Account.StartLink(activity, (int)RequestCode.LinkToDropboxRequest);
                    return;
                }
                Account = e.P0;
                StartApp(e.P1);
            };
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
            //Monkeys = GetMonkeys();
            //DrawMonkeys(Monkeys);
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



    //public interface IMvxDBTable
    //{
    //    IMvxDBRecord GetOrInsert(string id, DBFields fields = null);
    //}

    //public class MvxDBTable : IMvxDBTable
    //{
    //    private readonly DBTable DBTable;

    //    public MvxDBTable(DBTable dbTable)
    //    {
    //        DBTable = dbTable;
    //    }

    //    public IMvxDBRecord GetOrInsert(string id, IMvxDBFields fields = null)
    //    {

    //        return new MvxDBRecord(DBTable.GetOrInsert(id, (DBFields)fields));
    //    }
    //}

    //public interface IMvxDBFields
    //{
    //    IMvxDBFields DeleteField(string name);
    //}

    //public class MvxDBFields : IMvxDBFields
    //{
    //    private readonly DBFields DBFields;

    //    public MvxDBFields(DBFields dbFields)
    //    {
    //        DBFields = dbFields;
    //    }

    //    public IMvxDBFields DeleteField(string name)
    //    {
    //        return new MvxDBFields(DBFields.DeleteField(name));
    //    }

    //    public ICollection<string> FieldNames()
    //    {
    //        return DBFields.FieldNames();
    //    }

    //    public bool GetBoolean(string name) 
    //    {
    //        return DBFields.GetBoolean(name);
    //    }

    //    public byte[] GetBytes(string name)
    //    {
    //        return DBFields.GetBytes(name);
    //    }

    //    public DateTime GetDate(string name)
    //    {
    //        return DBFields.GetDate(name);
    //    }

    //    public bool GetBoolean(string name)
    //    {
    //        return DBFields.GetBoolean(name);
    //    }

    //    public bool GetBoolean(string name)
    //    {
    //        return DBFields.GetBoolean(name);
    //    }

    //}
    //public interface IMvxDBRecord : IMvxDBFields
    //{
    //    bool IsDeleted { get; }

    //    bool IsValidId(string id);

    //    bool IsValidFieldName(string id);
    //}

    //public class MvxDBRecord : MvxDBFields, IMvxDBRecord
    //{
    //    private readonly DBRecord DBRecord;

    //    public MvxDBRecord(DBRecord dbRecord) : base(dbRecord)
    //    {
    //        DBRecord = dbRecord;
    //    }

    //    public virtual bool IsDeleted { get { return DBRecord.IsDeleted; } }
    //}
}
