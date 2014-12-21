using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cham.MvvmCross.Plugins.DropBox
{
    public abstract class MvxDBRecordBase: IMvxDBRecord
    {
        public abstract string Id { get; }

        public abstract bool IsDeleted { get; }

        //public abstract bool IsValidId(string id);

        public abstract void DeleteRecord();

        public abstract object this[string fieldName] { get; set; }

        public abstract void DeleteField(string fieldName);

        //public abstract bool IsValidFieldName(string fieldName);

        public abstract ICollection<string> FieldNames
        {
            get;
        }
    }
}
