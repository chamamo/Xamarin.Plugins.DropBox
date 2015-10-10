using System.Collections.Generic;

namespace Cham.MvvmCross.Plugins.DropBox
{
    public interface IMvxDBFields
    {
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