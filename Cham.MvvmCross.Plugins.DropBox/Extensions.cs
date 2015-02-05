﻿using Cirrious.CrossCore;
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

        public static bool Populate<T>(this IMvxDBRecord record, ref T entityToPopulate) where T : IMvxDBEntity
        {
            var map = MvxDBMapping.Get(typeof(T));
            if (map == null || map.PropertiesInfos == null || map.PropertiesInfos.Count == 0) return false;
            var result = false;
            //Property Key
            var keyValue = map.PropertyInfoKey.GetValue(entityToPopulate) as string;
            if (keyValue != record.Id)
            {
                map.PropertyInfoKey.SetValue(entityToPopulate, record.Id);
                result = true;
            }
            //Others
            foreach (var fieldName in record.FieldNames)
            {
                var property = map.PropertiesInfos.FirstOrDefault(p => p.Name == fieldName);
                if (property != null)
                {
                    var oldValue = property.GetValue(entityToPopulate);
                    var value = record[fieldName];
                    if (oldValue != value)
                    {

                        property.SetValue(entityToPopulate, value.ConvertValue(property.PropertyType));
                        result = true;
                    }
                }
            }
            return result;
        }

        public static void UpdateFrom<T>(this IMvxDBRecord record, T sourceEntity) where T : IMvxDBEntity
        {
            if (sourceEntity == null) return;
            var map = MvxDBMapping.Get(sourceEntity.GetType());
            if (map == null || map.PropertiesInfos == null || map.PropertiesInfos.Count == 0) return;
            foreach (var property in map.PropertiesInfos)
            {
                var value = property.GetValue(sourceEntity);
                if (value == null) continue;
                if (record[property.Name] != value) record[property.Name] = value;
            }
        }

        public static IMvxDBRecord GetMvxDBRecord<TEntityType>(this TEntityType entity) where TEntityType : IMvxDBEntity
        {
            var fields = Mvx.Resolve<IMvxDBRecord>();
            if (entity == null) return null;
            var map = MvxDBMapping.Get(entity.GetType());
            if (map == null || map.PropertiesInfos == null || map.PropertiesInfos.Count == 0) return null;
            foreach (var property in map.PropertiesInfos)
            {
                var value = property.GetValue(entity);
                if (value == null) continue;
                fields[property.Name] = value;        
            }
            return fields;

        }

        public static T ConvertValue<T>(this object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static object ConvertValue(this object value, Type destinationType)
        {
            return Convert.ChangeType(value, destinationType);
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
