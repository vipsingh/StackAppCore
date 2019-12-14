using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Dapper;
using Model;
using Npgsql;
using System.Dynamic;

namespace DB
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

        public static IEnumerable<DynamicObj> ExecSelectQuery(string query, object param = null, IDbTransaction trans = null)
        {
            List<DynamicObj> rows = new List<DynamicObj>();
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var data = dbConnection.Query<dynamic>(query, param, trans);
                foreach (var row in data)
                {
                    var f = row as IDictionary<string, object>;
                    rows.Add(DynamicObj.From(f));
                }
            }

            return rows;
        }
    }
}
