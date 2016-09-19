using System;
using System.Collections.Generic;
using DropBoxSync.iOS;
using MonoTouch.Foundation;
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
            DBError error;
            var record = DBTable.GetRecord(id, out error);
            if (error != null) throw new Exception(error.ToString());
            return record.ToDbRecord();
        }

        public override IEnumerable<IDbRecord> Query(Dictionary<string, object> query = null)
        {
            NSDictionary dictionary = null;
            if (query != null)
            {
                dictionary = query.Convert();
                
            }
            DBError error;
            var records = DBTable.Query(dictionary, out error);
            foreach (var record in records)
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