using DropboxSync.Android;
using Xamarin.Plugins.DropBox.Abstractions;

namespace Xamarin.Plugins.DropBox
{
    public class DbRecord : DbFields<DBRecord>, IDbRecord
    {
        public DbRecord(DBRecord dbField)
            : base(dbField)
        {
        }

        public string Id => InternalDbFields.Id;

        public bool IsDeleted => InternalDbFields.IsDeleted;

        public void DeleteRecord()
        {
            InternalDbFields.DeleteRecord();
        }

    }
}