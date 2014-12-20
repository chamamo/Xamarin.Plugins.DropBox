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

        IMvxDBTable<T> GetTable<T>(string tableName) where T : IMvxDBEntity;

        IMvxDBTable<T> GetTable<T>() where T : IMvxDBEntity;

        void Init(string appKey, string appSecret);

        void Sync();
    }

    public interface IMvxDBTable<T> where T : IMvxDBEntity
    {
        void AddOrUpdate(T entity, string id, bool autoSync = true);

        void Delete(T entity, string id, bool autoSync = true);

        void AddOrUpdate(T entity, bool autoSync = true);

        void Delete(T entity, bool autoSync = true);
    }

    
}
