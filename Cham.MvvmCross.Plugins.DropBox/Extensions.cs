using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cham.MvvmCross.Plugins.DropBox
{
    public static class Extensions
    {

        public static bool Populate<T>(this Dictionary<string, object> dic, ref T entityToPopulate) where T : IMvxDBEntity
        {
            var map = MvxDBMapping.Get(typeof(T));
            if (map == null || map.PropertiesInfos == null || map.PropertiesInfos.Count == 0) return false;
            var result = false;
            foreach (var kpv in dic)
            {
                var property = map.PropertiesInfos.FirstOrDefault(p => p.Name == kpv.Key);
                if (property != null)
                {
                    var oldValue = property.GetValue(entityToPopulate);
                    if (oldValue != kpv.Value)
                    {
                        property.SetValue(entityToPopulate, kpv.Value);
                        result = true;
                    }
                }
            }
            return result;
        }

        public static T ConvertValue<T>(this object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static bool IsNumeric(this object value)
        {
            if (value == null) return false;
            return IsNumeric(value.GetType());
        }

        public static bool IsNumeric(this Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            return type == typeof(sbyte)
                    || type == typeof(byte)
                    || type == typeof(short)
                    || type == typeof(ushort)
                    || type == typeof(int)
                    || type == typeof(uint)
                    || type == typeof(long)
                    || type == typeof(ulong)
                    || type == typeof(float)
                    || type == typeof(double)
                    || type == typeof(decimal);
        }

        public static bool IsLong(this object value)
        {
            if (value == null) return false;
            return IsLong(value.GetType());
        }

        public static bool IsLong(this Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            return type == typeof(sbyte)
                    || type == typeof(byte)
                    || type == typeof(short)
                    || type == typeof(ushort)
                    || type == typeof(int)
                    || type == typeof(uint)
                    || type == typeof(long);
        }

        public static bool IsDateTime(this object value)
        {
            if (value == null) return false;
            return IsDateTime(value.GetType());

        }

        public static bool IsDateTime(this Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            return type == typeof(DateTime);
        }

        public static bool IsBool(this object value)
        {
            if (value == null) return false;
            return IsBool(value.GetType());

        }

        public static bool IsBool(this Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            return type == typeof(bool);
        }

        public static Type GetTypeOrUnderliningType(this object expression)
        {
            if (expression == null)
                return null;
            var objType = expression.GetType();
            return Nullable.GetUnderlyingType(objType) ?? objType;
        }
    }
}
