using System;
using System.Data;
using StackErp.Model;

namespace System.Data
{
    public static class DbExtensions
    {
        public static DynamicObj ReadAsDynamicObj(this IDataRecord reader)
        {
            var d = new DynamicObj();
            for(int i=0; i< reader.FieldCount; i++)
            {
                d.Add(reader.GetName(i), reader.GetValue(i));
            }

            return d;
        }

        public static DbObject ReadAsDbObject(this IDataRecord reader)
        {
            var d = new DbObject();
            for(int i=0; i< reader.FieldCount; i++)
            {
                d.Add(reader.GetName(i), reader.GetValue(i));
            }

            return d;
        }
    }
}