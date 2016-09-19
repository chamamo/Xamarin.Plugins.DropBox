using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Plugins.DropBox.Abstractions
{
    public interface IDbDataStore : IDisposable
    {
        event EventHandler<DbRecordChangedEventArgs> DbRecordChanged;

        bool HasLinkedAccount { get; }

        long Size { get; }

        long UnsyncedChangesSize { get; }

        long RecordCount { get; }

        void Unlink();

        IDbTable<T> GetTable<T>(string tableName) where T : IDbEntity;

        IDbTable<T> GetTable<T>() where T : IDbEntity;

        void Init(string appKey, string appSecret);

        void Sync();

        void Delete();

        void InitMapping(params Assembly[] assemblies);
    }
}
