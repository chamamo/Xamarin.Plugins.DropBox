using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DropboxSync.Android;

namespace Cham.MvvmCross.Plugins.DropBox.Droid
{
    public class MvxDBRecord : MvxDBRecordBase, IMvxDBRecord
    {
        private readonly DBFields DBFields;

        public MvxDBRecord()
        {
            DBFields = new DBFields();
        }

        public MvxDBRecord(DBFields dbField)
        {
            DBFields = dbField;
        }

        public override string Id
        {
            get
            {
                if (DBFields is DBRecord) return ((DBRecord)DBFields).Id;
                return null;
            }
        }

        public override bool IsDeleted
        {
            get
            {
                if (DBFields is DBRecord) return ((DBRecord)DBFields).IsDeleted;
                return false;
            }
        }

        public override void DeleteRecord()
        {
            if (DBFields is DBRecord) ((DBRecord)DBFields).DeleteRecord();
        }

        public override ICollection<string> FieldNames
        {
            get { return DBFields.FieldNames(); }
        }



        public override object this[string fieldName]
        {
            get
            {
                var type = DBFields.GetFieldType(fieldName);
                if (type == (DBFields.ValueType.Boolean))
                {
                    return DBFields.GetBoolean(fieldName);
                }
                else if (type == (DBFields.ValueType.Date))
                {
                    var date = DBFields.GetDate(fieldName);
                    return date.ToDateTime();
                }
                else if (type == (DBFields.ValueType.Double))
                {
                    return DBFields.GetDouble(fieldName);
                }
                else if (type == (DBFields.ValueType.List))
                {
                    throw new NotImplementedException();
                }
                else if (type == (DBFields.ValueType.Long))
                {
                    return DBFields.GetLong(fieldName);
                }
                else if (type == (DBFields.ValueType.String))
                {
                    return DBFields.GetString(fieldName);
                }
                else
                {
                    throw new NotImplementedException();
                }

            }
            set
            {
                var type = DBFields.GetFieldType(fieldName);
                if (type == (DBFields.ValueType.Boolean))
                {
                    DBFields.Set(fieldName, (bool)value);
                }
                else if (type == (DBFields.ValueType.Date))
                {
                   DBFields.Set(fieldName, ((DateTime)value).ToDate());
                }
                else if (type == (DBFields.ValueType.Double))
                {
                    DBFields.Set(fieldName, value.ConvertValue<double>());
                }
                else if (type == (DBFields.ValueType.List))
                {
                    throw new NotImplementedException();
                }
                else if (type == (DBFields.ValueType.Long))
                {
                    DBFields.Set(fieldName, value.ConvertValue<long>());
                }
                else if (type == (DBFields.ValueType.String))
                {
                    DBFields.Set(fieldName, value as string);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public override void DeleteField(string fieldName)
        {
            DBFields.DeleteField(fieldName);
        }

    }
}