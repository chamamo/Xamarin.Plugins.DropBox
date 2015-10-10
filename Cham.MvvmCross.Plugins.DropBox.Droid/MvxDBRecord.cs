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
    public class MvxDBRecord : MvxDBFields<DBRecord>, IMvxDBRecord
    {
        public MvxDBRecord(DBRecord dbField)
            : base(dbField)
        {
        }

        public string Id
        {
            get
            {
                return InternalDbFields.Id;
            }
        }

        public bool IsDeleted
        {
            get
            {
                return InternalDbFields.IsDeleted;
            }
        }

        public void DeleteRecord()
        {
            InternalDbFields.DeleteRecord();
        }

    }
}