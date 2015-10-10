using System;
using System.Collections;
using System.Collections.Generic;
using DropboxSync.Android;

namespace Cham.MvvmCross.Plugins.DropBox.Droid
{
    public abstract class MvxDBFields<T> : IMvxDBFields where T : DBFields
    {
        protected readonly T InternalDbFields;

        public MvxDBFields(T dbField)
        {
            InternalDbFields = dbField;
        }

        public ICollection<string> FieldNames
        {
            get { return InternalDbFields.FieldNames(); }
        }

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

    public class MvxDBFields : MvxDBFields<DBFields>
    {
        public MvxDBFields() : this(new DBFields())
        {
        }

        public MvxDBFields(DBFields dbField) : base(dbField)
        {
        }
    }
}