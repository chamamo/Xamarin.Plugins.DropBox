using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Cirrious.CrossCore;

namespace Cham.MvvmCross.Plugins.DropBox
{
    public class MvxDBMapping
    {
        private static IList<MvxDBMapping> Mappings = new List<MvxDBMapping>();
        
        public MvxDBMapping(Type type)
        {
            Type = type;
            TableName = type.Name;
            var attributes = type.GetCustomAttributes(typeof(MvxDBTableAttribute), true).Cast<MvxDBTableAttribute>().ToList();
            if (attributes.Any())
            {
                TableName = attributes[0].Name;
            }

            var query = RuntimeReflectionExtensions.GetRuntimeProperties(type);
            PropertyInfoKey = query.FirstOrDefault(p => p.GetCustomAttribute<MvxDBKeyAttribute>(true) != null);
            query = query.Where(p => p.CanRead && p.CanWrite && p.GetCustomAttribute<MvxDBIgnoreAttribute>(true) == null && p.GetCustomAttribute<MvxDBKeyAttribute>(true) == null);
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
            if (!Mappings.Any(m => m.Type == type)) Mappings.Add(new MvxDBMapping(type));
        }

        public static void Add<T>() where T : IMvxDBEntity
        {
            Add(typeof(T));
        }

        public static MvxDBMapping Get<T>() where T : IMvxDBEntity
        {
            return Get(typeof(T));
        }

        public static MvxDBMapping Get(Type type)
        {
            return Mappings.FirstOrDefault(m => m.Type == type);
        }

        public static MvxDBMapping Get(string name)
        {
            return Mappings.FirstOrDefault(m => m.TableName.Equals(name));
        }
    }
}
