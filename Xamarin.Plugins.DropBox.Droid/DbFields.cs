using System;
using System.Collections.Generic;
using DropboxSync.Android;
using Xamarin.Plugins.DropBox.Abstractions;

namespace Xamarin.Plugins.DropBox
{
    public abstract class DbFields<T> : IDbFields where T : DBFields
    {
        protected readonly T InternalDbFields;

        protected DbFields(T dbField)
        {
            InternalDbFields = dbField;
        }

        public ICollection<string> FieldNames => InternalDbFields.FieldNames();

        public object this[string fieldName]
        {
            get
            {
                var type = InternalDbFields.GetFieldType(fieldName);
                if (type == (DBFields.ValueType.Boolean))
                {
                    return InternalDbFields.GetBoolean(fieldName);
                }
                else if (type == (DBFields.ValueType.Date))
                {
                    var date = InternalDbFields.GetDate(fieldName);
                    return date.ToDateTime();
                }
                else if (type == (DBFields.ValueType.Double))
                {
                    return InternalDbFields.GetDouble(fieldName);
                }
                else if (type == (DBFields.ValueType.List))
                {
                    throw new NotImplementedException();
                }
                else if (type == (DBFields.ValueType.Long))
                {
                    return InternalDbFields.GetLong(fieldName);
                }
                else if (type == (DBFields.ValueType.String))
                {
                    return InternalDbFields.GetString(fieldName);
                }
                else
                {
                    throw new NotImplementedException();
                }

            }
            set
            {
                var type = InternalDbFields.GetFieldType(fieldName);
                if (type == (DBFields.ValueType.Boolean))
                {
                    InternalDbFields.Set(fieldName, (bool)value);
                }
                else if (type == (DBFields.ValueType.Date))
                {
                    InternalDbFields.Set(fieldName, ((DateTime)value).ToDate());
                }
                else if (type == (DBFields.ValueType.Double))
                {
                    InternalDbFields.Set(fieldName, value.ConvertValue<double>());
                }
                else if (type == (DBFields.ValueType.List))
                {
                    throw new NotImplementedException();
                }
                else if (type == (DBFields.ValueType.Long))
                {
                    InternalDbFields.Set(fieldName, value.ConvertValue<long>());
                }
                else if (type == (DBFields.ValueType.String))
                {
                    InternalDbFields.Set(fieldName, value as string);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void DeleteField(string fieldName)
        {
            InternalDbFields.DeleteField(fieldName);
        }
    }

    public class DbFields : DbFields<DBFields>
    {
        public DbFields() : this(new DBFields())
        {
        }

        public DbFields(DBFields dbField) : base(dbField)
        {
        }
    }
}