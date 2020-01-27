using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Dapper;
using StackErp.Model;
using Npgsql;
using System.Dynamic;

namespace StackErp.DB
{
    public static class DBService
    {
        private static string _connectionString;
        public static void Init(string connectionString)
        {
            _connectionString = connectionString;
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
            List<DbObject> rows = new List<DbObject>();
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var data = dbConnection.Query<dynamic>(query, param, trans);
                foreach (var row in data)
                {
                    var f = row as IDictionary<string, object>;
                    rows.Add(DbObject.From(f));
                }
            }

            return rows;
        }

        public static DbObject Single(string query, object param = null, IDbTransaction trans = null)
        {
            DbObject row = new DbObject();
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var data = dbConnection.QuerySingle<dynamic>(query, param, trans);
                var f = data as IDictionary<string, object>;
                row = DbObject.From(f);
            }

            return row;
        }

        public static int Execute(string query, object param, IDbTransaction trans = null)
        {
            using (IDbConnection dbConnection = Connection)
            {
                var affectedRows = dbConnection.Execute(query, param);

                return affectedRows;
            }
        }

        public static DbType GetDbType(BaseTypeCode type)
        {
            var t = (int)type;
        
            return (DbType)t;
        }
    }
}
