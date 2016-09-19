using System;
using System.Collections.Generic;
using System.Linq;
using DropBoxSync.iOS;
using Xamarin.Plugins.DropBox.Abstractions;
using MonoTouch.Foundation;

namespace Xamarin.Plugins.DropBox
{
    public class DbRecord : IDbRecord
    {
        private readonly DBRecord InternalDbField;

        public DbRecord(DBRecord dbField)
        {
            InternalDbField = dbField;
        }

        public ICollection<string> FieldNames
        {
            get
            {
                return InternalDbField.Fields.Select(item => (NSString) item.Key).Select(dummy => (string) dummy).ToList();
            }
        }

        public object this[string fieldName]
        {
            get
            {
                //var type = InternalDbField.GetFieldType(fieldName);
                //if (type == (DBFields.ValueType.Boolean))
                //{
                //    return InternalDbFields.GetBoolean(fieldName);
                //}
                //else if (type == (DBFields.ValueType.Date))
                //{
                //    var date = InternalDbFields.GetDate(fieldName);
                //    return date.ToDateTime();
                //}
                //else if (type == (DBFields.ValueType.Double))
                //{
                //    return InternalDbFields.GetDouble(fieldName);
                //}
                //else if (type == (DBFields.ValueType.List))
                //{
                //    throw new NotImplementedException();
                //}
                //else if (type == (DBFields.ValueType.Long))
                //{
                //    return InternalDbFields.GetLong(fieldName);
                //}
                //else if (type == (DBFields.ValueType.String))
                //{
                //    return InternalDbFields.GetString(fieldName);
                //}
                //else
                {
                    throw new NotImplementedException();
                }

            }
            set
            {
                //var type = InternalDbFields.GetFieldType(fieldName);
                //if (type == (DBFields.ValueType.Boolean))
                //{
                //    InternalDbFields.Set(fieldName, (bool)value);
                //}
                //else if (type == (DBFields.ValueType.Date))
                //{
                //    InternalDbFields.Set(fieldName, ((DateTime)value).ToDate());
                //}
                //else if (type == (DBFields.ValueType.Double))
                //{
                //    InternalDbFields.Set(fieldName, value.ConvertValue<double>());
                //}
                //else if (type == (DBFields.ValueType.List))
                //{
                //    throw new NotImplementedException();
                //}
                //else if (type == (DBFields.ValueType.Long))
                //{
                //    InternalDbFields.Set(fieldName, value.ConvertValue<long>());
                //}
                //else if (type == (DBFields.ValueType.String))
                //{
                //    InternalDbFields.Set(fieldName, value as string);
                //}
                //else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void DeleteField(string fieldName)
        {
            InternalDbField.RemoveObject(fieldName);
        }

        

        public string Id => InternalDbField.RecordId;

        public bool IsDeleted => InternalDbField.Deleted;

        public void DeleteRecord()
        {
            InternalDbField.DeleteRecord();
        }

    }
}