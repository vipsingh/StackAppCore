using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Dapper;
using StackErp.Model;
using Npgsql;
using System.Dynamic;
using System.Data.Common;
using NpgsqlTypes;

namespace StackErp.DB
{    
    public static class DBService
    {
        private static string _connectionString;
        public static void Init(string connectionString)
        {
            _connectionString = connectionString;
            
            //SqlMapper.ResetTypeHandlers();
            //SqlMapper.AddTypeHandler(new ArrayTypeHanler());
        }

        internal static IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(_connectionString);
            }
        }

        public static IEnumerable<DbObject> Query(string query, object param = null, IDbTransaction trans = null)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var data = dbConnection.Query<dynamic>(query, param, trans);
                return ConvertToDbObjectData(data);
            }
        }

        public static List<DbObject> ConvertToDbObjectData(IEnumerable<dynamic> data)
        {
            List<DbObject> rows = new List<DbObject>();
            foreach (var row in data)
            {
                var f = row as IDictionary<string, object>;
                rows.Add(DbObject.From(f));
            }
            return rows;
        }

        public static void Query(string query, object param, Action<IDataRecord> onRowRead, IDbTransaction trans = null)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                using(var reader = (DbDataReader)dbConnection.ExecuteReader(query, param))
                {
                    while (reader.Read())
                    {
                        onRowRead(reader);
                    }
                }
            }
        }

        public static DbObject Single(string query, object param = null, IDbTransaction trans = null)
        {
            DbObject row = new DbObject();
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var ds = dbConnection.Query<dynamic>(query, param, trans);
                if (ds.Count() > 0)
                {
                    var f = ds.First() as IDictionary<string, object>;
                    row = DbObject.From(f);
                } else {
                    return null;
                }
            }

            return row;
        }

        public static InvariantDictionary<IEnumerable<DbObject>> Query(InvariantDictionary<string> sqls, object param = null, IDbTransaction trans = null)
        {
            InvariantDictionary<IEnumerable<DbObject>> rows = new InvariantDictionary<IEnumerable<DbObject>>();
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                foreach(var q in sqls)
                {
                    var data = dbConnection.Query<dynamic>(q.Value, param, trans);
                    rows.Add(q.Key, ConvertToDbObjectData(data));
                }
            }

            return rows;
        }

        public static void QueryMultiple(string query, object param, Action<SqlMapper.GridReader> onRead, IDbTransaction trans = null)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                using (var multi = dbConnection.QueryMultiple(query, param))
                {
                    onRead(multi);
                }
            }
        }

        public static int Execute(string query, object param, IDbTransaction trans = null)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var affectedRows = dbConnection.Execute(query, param);

                return affectedRows;
            }
        }

        public static DbType GetDbType(FieldType fieldType, TypeCode type)
        {
            DbType t = DbType.String;
            switch(type) {
                case TypeCode.Object:
                    t = DbType.Object;
                    break;
                case TypeCode.Boolean:
                 t = DbType.Boolean;
                    break;
                case TypeCode.Char:
                 t = DbType.StringFixedLength;
                    break;
                case TypeCode.SByte:
                 t = DbType.SByte;
                    break;
                case TypeCode.Byte:
                 t = DbType.Byte;
                    break;
                case TypeCode.Int16:
                    t = DbType.Int16;
                    break;
                case TypeCode.UInt16:
                                 t = DbType.UInt16;
                    break;
                case TypeCode.Int32:
                                 t = DbType.Int32;
                    break;
                case TypeCode.UInt32:
                                 t = DbType.UInt32;
                    break;
                case TypeCode.Int64:
                                 t = DbType.Int64;
                    break;
                case TypeCode.UInt64:
                                 t = DbType.UInt64;
                    break;
                case TypeCode.Single:
                                 t = DbType.Single;
                    break;
                case TypeCode.Double:
                                 t = DbType.Double;
                    break;
                case TypeCode.Decimal:
                                 t = DbType.Decimal;
                    break;
                case TypeCode.DateTime:
                    t = DbType.DateTime;
                    break;


            }
        
            return t;
        }
        
    }

    public class DbObjectTypeHanler: SqlMapper.TypeHandler<DbObject>
    {
        public override DbObject Parse(object value)
        {
            var d = new DbObject();

            return d;
        }
        public override void SetValue(IDbDataParameter parameter, DbObject value)
        {

        }
    }

    // public class DbIntArrayColData
    // {
    //     public int[] _d;
    //     public DbIntArrayColData(IEnumerable<int> d)
    //     {
    //         _d = d.ToArray();
    //     }
    // }
    // public class ArrayTypeHanler: SqlMapper.TypeHandler<DbIntArrayColData>
    // {
    //     public override DbIntArrayColData Parse(object value)
    //     {
    //         return null;
    //     }

    //     public override void SetValue(IDbDataParameter parameter, DbIntArrayColData value)
    //     {
    //         if (parameter is NpgsqlParameter)
    //         {
    //             var p = (NpgsqlParameter)parameter;
    //             p.Value = value._d;
    //             p.NpgsqlDbType = NpgsqlDbType.Array | NpgsqlDbType.Integer;                
    //         }
    //         else 
    //         {
    //             parameter.Value = string.Join(",", value);
    //         }

            
    //     }
    // }
}
