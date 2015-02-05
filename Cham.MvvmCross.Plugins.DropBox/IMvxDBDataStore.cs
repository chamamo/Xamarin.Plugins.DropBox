using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cham.MvvmCross.Plugins.DropBox
{
    public interface IMvxDBDataStore
    {
        bool HasLinkedAccount { get; }

        void Unlink();

        IMvxDBTable<T> GetTable<T>(string tableName) where T : IMvxDBEntity;

        IMvxDBTable<T> GetTable<T>() where T : IMvxDBEntity;

        void Init(string appKey, string appSecret);

        void Sync();

        void Delete();
    }

    public interface IMvxDBTable<T> where T : IMvxDBEntity
    {
        IMvxDBRecord Get(string id);

        IMvxDBRecord GetOrInsert(T entity, string id, bool autoSync = true);

        void Delete(T entity, string id, bool autoSync = true);

        IMvxDBRecord GetOrInsert(T entity, bool autoSync = true);

        void Delete(T entity, bool autoSync = true);

        void Delete(IMvxDBRecord record, bool autoSync = true);

        IEnumerable<IMvxDBRecord> Query(Dictionary<string, object> query = null);
    }

    
}
