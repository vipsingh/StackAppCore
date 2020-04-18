using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.DB
{
    public static partial class EntityDBService
    {
        public static AnyStatus SaveEntity(IDBEntity entity, EntityModelBase model)
        {
            var sts = AnyStatus.UpdateFailure;

            var eSave = new EntityDBSave(entity);

            if(model.IsNew) {
                var eId = GetNextEntityDBId(entity.EntityId.Code);
                model.SetID(eId);
            }

            using (IDbConnection connection = DBService.Connection)
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    eSave.Save(model, connection, transaction);
                    transaction.Commit();
                    
                    sts = AnyStatus.Success;                    
                }
            }

            return sts;         
        }
    }
    public class EntityDBSave
    {
        private IDBEntity _entity;
        private EntityModelBase _model;        

        public EntityDBSave(IDBEntity entity)
        {
            _entity = entity;
        }
        public int Save(EntityModelBase model, IDbConnection connection, IDbTransaction transaction)
        {
            _model = model;
            var parameters = new DynamicParameters();
            var toUpdateFields = PrepareInsertUpdateFields(_model);
            var qry = BuildQuery(model, toUpdateFields, ref parameters);

            return connection.Execute(qry, parameters, transaction);
        }

        internal static string BuildQuery(DBModelBase model, List<(string, DynamicDbParam)> fls, ref DynamicParameters parameters)
        {
            string qry = "";
            
            List<string> toInsert = new List<string>();
            List<string> toInsertP = new List<string>();

            foreach (var f in fls)
            {
                if (!parameters.ParameterNames.Contains(f.Item2.Name))
                    parameters.AddParam(f.Item2);
            }

            toInsert.Add("ID");
            toInsertP.Add("@ID");
            parameters.AddParam(new DynamicDbParam("ID", model.ID, DbType.Int32));

            if (!model.IsNew) {
                return CreateUpdateQuery(model.DbTableName, fls);
            }


            foreach (var f in fls)
            { 
                if(!toInsert.Contains(f.Item1, StringComparer.InvariantCultureIgnoreCase))
                {
                    toInsert.Add(f.Item1);
                    toInsertP.Add("@" + f.Item2.Name);
                }
            }

            qry = $"INSERT INTO {model.DbTableName}({String.Join(",", toInsert)}) VALUES({String.Join(",", toInsertP)})";
            return qry;
        }

        private static string CreateUpdateQuery(string tableName, List<(string, DynamicDbParam)> fields) 
        {
            var q = $"UPDATE {tableName} SET ";
            List<string> toUpdate = new List<string>();
            List<string> toUpdate1 = new List<string>();
            foreach (var f in fields)
            {
                 if(!toUpdate1.Contains(f.Item1, StringComparer.InvariantCultureIgnoreCase))
                {
                    toUpdate.Add($" {f.Item1} = @{f.Item2.Name} ");
                    toUpdate1.Add(f.Item1);
                }
            }

            q = q + String.Join(",", toUpdate);

            q = q + " WHERE id=@ID;";

            return q;
        }

        private static List<(string, DynamicDbParam)> PrepareInsertUpdateFields(EntityModelBase model)
        {
            List<(string, DynamicDbParam)> qryA = new List<(string, DynamicDbParam)>();
            foreach (var f in model.Attributes)
            {
                var attr = f.Value;
                var dbName = attr.Field.DBName;

                if (attr.IsChanged)
                {
                    var param = new DynamicDbParam(f.Key, attr.Value, DBService.GetDbType(attr.Field.BaseType));
                    qryA.Add((dbName, param));
                }
            }

            return qryA;
        }
    }
}