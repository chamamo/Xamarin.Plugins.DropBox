using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Plugins.DropBox.Abstractions;

namespace Xamarin.Plugins.DropBox
{
    class DbDataStore : DbDataStoreBase, IDbDataStore
    {
        public event EventHandler<DbRecordChangedEventArgs> DbRecordChanged;
        public bool HasLinkedAccount { get; }
        public long Size { get; }
        public long UnsyncedChangesSize { get; }
        public long RecordCount { get; }
        public void Unlink()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void Sync()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
