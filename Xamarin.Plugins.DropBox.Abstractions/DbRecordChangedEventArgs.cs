using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Plugins.DropBox.Abstractions
{
    public class DbRecordChangedEventArgs : EventArgs
    {
        public readonly IDbRecord DbRecord;

        public DbRecordChangedEventArgs(IDbRecord dbRecord)
        {
            DbRecord = dbRecord;
        }
    }
}
