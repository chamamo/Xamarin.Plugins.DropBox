using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Xamarin.Plugins.DropBox.Abstractions
{
    public class DbMapping
    {
        private static readonly IList<DbMapping> Mappings = new List<DbMapping>();
        
        public DbMapping(Type type)
        {
            Type = type;
            TableName = type.Name;
            var attributes = type.GetTypeInfo().GetCustomAttributes(typeof(DbTableAttribute), true).Cast<DbTableAttribute>().ToList();
            if (attributes.Any())
            {
                TableName = attributes[0].Name;
            }

            var query = type.GetRuntimeProperties();
            PropertyInfoKey = query.FirstOrDefault(p => p.GetCustomAttribute<DbKeyAttribute>(true) != null);
            query = query.Where(p => p.CanRead && p.CanWrite && p.GetCustomAttribute<DbIgnoreAttribute>(true) == null && p.GetCustomAttribute<DbKeyAttribute>(true) == null);
            PropertiesInfos = query.ToList();
        }

        public Type Type { get; private set; }

        public string TableName { get; private set; }

        public IList<PropertyInfo> PropertiesInfos { get; private set; }

        public PropertyInfo PropertyInfoKey
        {
            get;
            private set;
        }

        public static void Add(Type type)
        {
            if (Mappings.All(m => m.Type != type)) Mappings.Add(new DbMapping(type));
        }

        public static void Clear()
        {
            Mappings.Clear();
        }

        public static void Add<T>() where T : IDbEntity
        {
            Add(typeof(T));
        }

        public static DbMapping Get<T>() where T : IDbEntity
        {
            return Get(typeof(T));
        }

        public static DbMapping Get(Type type)
        {
            return Mappings.FirstOrDefault(m => m.Type == type);
        }

        public static DbMapping Get(string name)
        {
            return Mappings.FirstOrDefault(m => m.TableName.Equals(name));
        }
    }
}
