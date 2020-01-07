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
        public static void SaveEntity(IDBEntity entity, EntityModelBase model)
        {
            var eSave = new EntityDBSave(entity);

            var eId = GetNextEntityDBId(entity.Name);
            model.SetID(eId);

            using (IDbConnection connection = DBService.Connection)
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    eSave.Save(model, connection, transaction);
                    transaction.Commit();
                }
            }            
        }
    }
    public class EntityDBSave
    {
        private IDBEntity _entity;
        private EntityModelBase _model;

        private DynamicParameters _parameters;

        public EntityDBSave(IDBEntity entity)
        {
            _entity = entity;
        }
        public int Save(EntityModelBase model, IDbConnection connection, IDbTransaction transaction)
        {
            _model = model;
            _parameters = new DynamicParameters();
            var qry = BuildQuery();

            return connection.Execute(qry, _parameters, transaction);
        }

        private string BuildQuery()
        {
            string qry = "";
            var fls = BuildInsertFields();
            List<string> toInsert = new List<string>();
            List<string> toInsertP = new List<string>();
            toInsert.Add("ID");
            toInsertP.Add("@ID");
            _parameters.AddParam(new DynamicDbParam("ID", _model.ID, DbType.Int32));


            foreach (var f in fls)
            {
                if (!_parameters.ParameterNames.Contains(f.Item2.Name))
                    _parameters.AddParam(f.Item2);    
                if(!toInsert.Contains(f.Item1))
                {
                    toInsert.Add(f.Item1);
                    toInsertP.Add("@" + f.Item2.Name);
                }
            }

            qry = $"INSERT INTO {_entity.DBName}({String.Join(",", toInsert)}) VALUES({String.Join(",", toInsertP)})";
            return qry;
        }

        private List<(string, DynamicDbParam)> BuildInsertFields()
        {
            List<(string, DynamicDbParam)> qryA = new List<(string, DynamicDbParam)>();
            foreach (var f in _model.Attributes)
            {
                var attr = f.Value;
                var dbName = attr.Field.DBName;

                var param = new DynamicDbParam(f.Key, attr.Value, DBService.GetDbType(attr.Field.BaseType));
                qryA.Add((dbName, param));
            }

            return qryA;
        }
    }
}