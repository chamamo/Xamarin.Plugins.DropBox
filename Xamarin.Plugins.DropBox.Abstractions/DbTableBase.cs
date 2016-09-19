using System;
using System.Collections.Generic;

namespace Xamarin.Plugins.DropBox.Abstractions
{
    public abstract class DbTableBase<T> : IDbTable<T> where T : IDbEntity
    {
        public readonly IDbDataStore Store;

        protected DbTableBase(IDbDataStore store)
        {
            Store = store;
        }

        public abstract IDbRecord Get(string id);

        public abstract IDbRecord GetOrInsert(T entity, string id, bool autoSync = true);

        public abstract void Delete(T entity, string id, bool autoSync = true);

        public IDbRecord GetOrInsert(T entity, bool autoSync = true)
        {
            var map = DbMapping.Get(typeof(T));
            if (map.PropertyInfoKey == null) new KeyNotFoundException(string.Format("Type: {0} does not have MvxDBKey attribute", typeof(T).Name));
            var value = map.PropertyInfoKey.GetValue(entity);
            if (value == null) new NullReferenceException(string.Format("Type:{0}: Property key {1} is null", typeof(T).Name, map.PropertyInfoKey.Name));
            return GetOrInsert(entity, value.ToString(), autoSync);
        }

        public void Delete(T entity, bool autoSync = true)
        {
            var map = DbMapping.Get(typeof(T));
            if (map.PropertyInfoKey == null) new KeyNotFoundException(string.Format("Type: {0} does not have MvxDBKey attribute", typeof(T).Name));
            var value = map.PropertyInfoKey.GetValue(entity);
            if (value == null) new NullReferenceException(string.Format("Type:{0}: Property key {1} is null", typeof(T).Name, map.PropertyInfoKey.Name));
            Delete(entity, value.ToString(), autoSync);
        }

        public void Delete(IDbRecord record, bool autoSync = true)
        {
            record.DeleteRecord();
            if (autoSync) Store.Sync();
        }

        public abstract IEnumerable<IDbRecord> Query(Dictionary<string, object> query = null);
    }
}
