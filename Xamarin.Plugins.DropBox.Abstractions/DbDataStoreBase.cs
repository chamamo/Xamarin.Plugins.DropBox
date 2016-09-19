using System;
using System.Linq;
using System.Reflection;

namespace Xamarin.Plugins.DropBox.Abstractions
{
    public abstract class DbDataStoreBase : IDisposable
    {
        public void InitMapping(params Assembly[] assemblies)
        {
            DbMapping.Clear();

            foreach (var assembly in assemblies)
            {
                var query = assembly.DefinedTypes.Where(t => typeof(IDbEntity).GetTypeInfo().IsAssignableFrom(t));
                foreach (var type in query)
                {
                    DbMapping.Add(type.AsType());
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose up
        /// </summary>
        ~DbDataStoreBase()
        {
            Dispose(false);
        }
        private bool disposed = false;
        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose only
                }

                disposed = true;
            }
        }
    }
}
