using System.Collections.Generic;

namespace Xamarin.Plugins.DropBox.Abstractions
{
    public abstract class DbRecordBase: IDbRecord
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
