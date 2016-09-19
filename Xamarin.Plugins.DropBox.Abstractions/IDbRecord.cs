namespace Xamarin.Plugins.DropBox.Abstractions
{
    public interface IDbRecord : IDbFields
    {
        string Id { get; }

        bool IsDeleted { get; }


        void DeleteRecord();

    }
}
