using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using DropboxSync.Android;
using Java.Util;
using Xamarin.Plugins.DropBox.Abstractions;

namespace Xamarin.Plugins.DropBox
{
    public static class Extensions
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime ToDateTime(this Date date)
        {
            try
            {
                var ddd = UnixEpoch.AddMilliseconds(date.Time);
                var final = ddd.ToUniversalTime();
                return final;
            }
            catch { return DateTime.Now; }
        }

        public static Date ToDate(this DateTime dateTime)
        {
            return new Date(Convert.ToInt64((dateTime.ToUniversalTime() - UnixEpoch).TotalMilliseconds));
        }
        
        public static DBFields GetDbFields<TEntityType>(this TEntityType entity) where TEntityType : IDbEntity
        {
            if (entity == null) return null;
            var map = DbMapping.Get(entity.GetType());
            if (map?.PropertiesInfos == null || map.PropertiesInfos.Count == 0) return null;
            var fields = new DBFields();
            foreach (var property in map.PropertiesInfos)
            {
                var value = property.GetValue(entity);
                if (value == null) continue;
                fields.SetEx(property.Name, value);         
            }
            return fields;

        }

        public static Dictionary<string, object> ToDictionary(this DBRecord record, DbMapping mapping)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (var fieldName in record.FieldNames())
            {
                var property = mapping.PropertiesInfos.SingleOrDefault(p => p.Name.Equals(fieldName));
                if (property != null)
                {
                    object value = null;
                    var type = property.PropertyType;
                    if (type.IsLong()) value = record.GetLong(fieldName);
                    else if (type.IsNumeric()) value = record.GetDouble(fieldName);
                    else if (type.IsBool()) value = record.GetBoolean(fieldName);
                    else if (type.IsDateTime())
                    {
                        var date = record.GetDate(fieldName);
                        value =  date.ToDateTime();
                    }
                    else if (type == typeof(string)) value = record.GetString(fieldName);
                    else
                    {
                        throw new NotImplementedException();
                    }
                    dictionary.Add(fieldName, value);
                }
            }
            return dictionary;
        }

        public static DbRecord ToDbRecord(this DBRecord record)
        {
            if (record == null) return null;
            var map = DbMapping.Get(record.Table.Id);
            return new DbRecord(record);
        }

        public static DBFields ToDBFields(this Dictionary<string, object> dictionary)
        {
            var dbFields = new DBFields();
            foreach (var kpv in dictionary)
           {
               if (kpv.Value == null || kpv.Key == null) throw new NullReferenceException("Key or value is null");
               dbFields.SetEx(kpv.Key, kpv.Value);
           }
            return dbFields;
        }

        public static void SetEx(this DBFields fields, string name, object value)
        {
            if (value.IsNumeric()) fields.Set(name, value.ConvertValue<double>());
            else if (value.IsNumeric()) fields.Set(name, value.ConvertValue<long>());
            else if (value.IsBool()) fields.Set(name, value.ConvertValue<bool>());
            else if (value.IsDateTime())
            {
                var dateTime = value.ConvertValue<DateTime>();
                fields.Set(name, dateTime.ToDate());
            }
            else if (value is string) fields.Set(name, (string)value);
            else throw new NotImplementedException();   
        }
    }
}