using System.Collections.Generic;
using DropboxSync.Android;
using Xamarin.Plugins.DropBox.Abstractions;

namespace Xamarin.Plugins.DropBox
{
    public class DbTable<T> : DbTableBase<T> where T : IDbEntity
    {
        public readonly DBTable DBTable;

        public DbTable(DBTable table, IDbDataStore store)
            : base(store)
        {
            DBTable = table;
        }

        public override IDbRecord Get(string id)
        {
            return DBTable.Get(id).ToDbRecord();
        }

        public override IEnumerable<IDbRecord> Query(Dictionary<string, object> query = null)
        {
            DropboxSync.Android.DBTable.QueryResult result = null;
            if (query == null)
            {
                result = DBTable.Query();
            }
            else
            {
                var dbFields = query.ToDBFields();
                result = DBTable.Query(dbFields);
            }
            foreach (var record in result.AsList())
            {
                yield return record.ToDbRecord();
            }
        }

        public override IDbRecord GetOrInsert(T entity, string id, bool autoSync = true)
        {
            var dbFields = entity.GetDbFields<T>();
            var record = DBTable.GetOrInsert(id, dbFields);
            if (autoSync) Store.Sync();
            return record.ToDbRecord();
        }

        public override void Delete(T entity, string id, bool autoSync = true)
        {
            var record = DBTable.Get(id);
            if (record != null)
            {
                record.DeleteRecord();
                if(autoSync) Store.Sync();
            }
        }
    }
}