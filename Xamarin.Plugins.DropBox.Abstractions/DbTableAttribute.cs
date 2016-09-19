using System;

namespace Xamarin.Plugins.DropBox.Abstractions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DbTableAttribute : Attribute
    {
        public readonly string Name;

        public DbTableAttribute(string name)
        {
            Name = name;
        }
    }
}
