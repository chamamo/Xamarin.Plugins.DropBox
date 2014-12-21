using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cham.MvvmCross.Plugins.DropBox
{
    public abstract class MvxDBTableBase<T> : IMvxDBTable<T> where T : IMvxDBEntity
    {
        protected IMvxDBDataStore Store;

        protected MvxDBTableBase(IMvxDBDataStore store)
        {
            Store = store;
        }

        public abstract IMvxDBRecord Get(string id);

        public abstract IMvxDBRecord GetOrInsert(T entity, string id, bool autoSync = true);

        public abstract void Delete(T entity, string id, bool autoSync = true);

        public IMvxDBRecord GetOrInsert(T entity, bool autoSync = true)
        {
            var map = MvxDBMapping.Get(typeof(T));
            if (map.PropertyInfoKey == null) new KeyNotFoundException(string.Format("Type: {0} does not have MvxDBKey attribute", typeof(T).Name));
            var value = map.PropertyInfoKey.GetValue(entity);
            if (value == null) new NullReferenceException(string.Format("Type:{0}: Property key {1} is null", typeof(T).Name, map.PropertyInfoKey.Name));
            return GetOrInsert(entity, value.ToString(), autoSync);
        }

        public void Delete(T entity, bool autoSync = true)
        {
            var map = MvxDBMapping.Get(typeof(T));
            if (map.PropertyInfoKey == null) new KeyNotFoundException(string.Format("Type: {0} does not have MvxDBKey attribute", typeof(T).Name));
            var value = map.PropertyInfoKey.GetValue(entity);
            if (value == null) new NullReferenceException(string.Format("Type:{0}: Property key {1} is null", typeof(T).Name, map.PropertyInfoKey.Name));
            Delete(entity, value.ToString(), autoSync);
        }

        public abstract IEnumerable<IMvxDBRecord> Query(Dictionary<string, object> query = null);
    }
}
