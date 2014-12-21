using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cham.MvvmCross.Plugins.DropBox
{
    public interface IMvxDBRecord
    {
        string Id { get; }

        bool IsDeleted { get; }

        void DeleteRecord();

        ICollection<string> FieldNames
        {
            get;
        }



        object this[string fieldName]
        {
            get;
            set;
        }

        void DeleteField(string fieldName);
    }
}
