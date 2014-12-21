using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DropboxSync.Android;

namespace Cham.MvvmCross.Plugins.DropBox.Droid
{
    public class MvxDBTable<T> : MvxDBTableBase<T>, IMvxDBTable<T> where T : IMvxDBEntity
    {
        public readonly DBTable DBTable;

        public MvxDBTable(DBTable table, IMvxDBDataStore store)
            : base(store)
        {
            DBTable = table;
        }

        public override IMvxDBRecord Get(string id)
        {
            return DBTable.Get(id).ToMvxDBRecord();
        }

        public override IEnumerable<IMvxDBRecord> Query(Dictionary<string, object> query = null)
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
                yield return record.ToMvxDBRecord();
            }
        }

        public override IMvxDBRecord GetOrInsert(T entity, string id, bool autoSync = true)
        {
            var dbFields = entity.GetDBFields<T>();
            var record = DBTable.GetOrInsert(id, dbFields);
            if (autoSync) Store.Sync();
            return record.ToMvxDBRecord();
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