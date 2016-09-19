using Xamarin.Plugins.DropBox;
using Xamarin.Plugins.DropBox.Abstractions;

namespace DropBoxSample.Core.ViewModels
{
    public class Item : IDbEntity
    {
        [DbKey]
        public string Id { get; set; }

        public string Value { get; set; }

        [DbIgnore]
        public bool Changed { get; set; }
    }
}