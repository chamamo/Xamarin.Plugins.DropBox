using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Plugins.DropBox.Abstractions;

namespace Xamarin.Plugins.DropBox
{
    public class CrossDbDataStore
    {
        static Lazy<IDbDataStore> Implementation = new Lazy<IDbDataStore>(CreateDbDataStore, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static IDbDataStore Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static IDbDataStore CreateDbDataStore()
        {
#if PORTABLE
            return null;
#else
            return new DbDataStore();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }


        /// <summary>
        /// Dispose of everything 
        /// </summary>
        public static void Dispose()
        {
            if (Implementation != null && Implementation.IsValueCreated)
            {
                Implementation.Value.Dispose();

                Implementation = new Lazy<IDbDataStore>(CreateDbDataStore, System.Threading.LazyThreadSafetyMode.PublicationOnly);
            }
        }
    }
}
