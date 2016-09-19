using System.Collections.Generic;

namespace Xamarin.Plugins.DropBox.Abstractions
{
    public interface IDbFields
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