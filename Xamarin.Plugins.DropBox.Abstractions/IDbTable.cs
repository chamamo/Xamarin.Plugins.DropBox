using System.Collections.Generic;

namespace Xamarin.Plugins.DropBox.Abstractions
{
    public interface IDbTable<T> where T : IDbEntity
    {
        IDbRecord Get(string id);

        IDbRecord GetOrInsert(T entity, string id, bool autoSync = true);

        void Delete(T entity, string id, bool autoSync = true);

        IDbRecord GetOrInsert(T entity, bool autoSync = true);

        void Delete(T entity, bool autoSync = true);

        void Delete(IDbRecord record, bool autoSync = true);

        IEnumerable<IDbRecord> Query(Dictionary<string, object> query = null);
    }
}