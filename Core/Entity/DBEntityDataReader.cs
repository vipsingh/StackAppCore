using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using StackErp.Core.Form;
using StackErp.DB;
using StackErp.DB.DataList;
using StackErp.Model;
using StackErp.Model.DataList;
using StackErp.Model.Entity;

namespace StackErp.Core
{
    public partial class DBEntity : IDBEntity
    {        
        public EntityModelBase GetDefault()
        {
            EntityRecordModel model = new EntityRecordModel(this);
            model.CreateDefault();

            return model;
        }

        #region Data Fetch
        public virtual EntityModelBase GetSingle(int id)
        {
            var sql = _detailQry;
            var arr = DBService.Query(sql, new { ItemId = new int[] {id} });
            //var relatedFieldData = DBService.Query(_relatedFieldDataQryList, new { ItemId = new int[] {id} });
            if (arr.Count() > 0)
            {
                var d = arr.First();
                return BuildModelFromDbObj(d);
            }
            throw new EntityException("Record not found.");
        }

        private EntityRecordModel BuildModelFromDbObj(DbObject dbObj) //InvariantDictionary<IEnumerable<DbObject>> relatedFieldData
        {
            var model = new EntityRecordModel(this);
            // foreach(var fData in relatedFieldData)
            // {
            //     dbObj.Add(fData.Key + "__data", fData.Value);
            // }
            model.BuiltWithDB(dbObj);

            return model;
        }

        public List<EntityModelBase> GetAll(FilterExpression filter)
        {
            throw new NotImplementedException();
        }

        public List<EntityModelBase> GetAll(int[] ids)
        {
            var sql = _detailQry;
            var arr = DBService.Query(sql, new { ItemId = ids });
            var list = new List<EntityModelBase>();
            if (arr.Count() > 0)
            {
                foreach(var a in arr)
                {
                    var model = new EntityRecordModel(this);
                    model.BuiltWithDB(a);
                    list.Add(model);
                }
            }

            return list;
        }

        public DBModelBase Read(int id, List<string> fields)
        {
            var exp = new FilterExpression(this.EntityId);
            exp.Add(new FilterExpField(this.IDField, FilterOperationType.Equal, id));

            var d = ReadAll(fields, exp);
            if (d.Count > 0)
                return d.First();

            throw new UserException("Record is not available with id: " + id.ToString());
        }

        public List<DBModelBase> ReadAll(List<string> fields, FilterExpression filter)
        {
            var q = new DbQuery(this);
            foreach(var f in fields)
            {
                q.AddField(f, true);
            }
            q.AddField(this.IDField, true);
            
            q.SetFixedFilter(filter);

            var data = QueryDbService.ExecuteEntityQuery(q);   
            var models = new List<DBModelBase>();
            foreach(var d in data)
            {
                var m = new DBModelBase(this);
                m.BuiltWithDB(d);
                models.Add(m);
            }         

            return models;
        }

        //used in script
        public List<DBModelBase> ReadAll(List<string> fields, string filter)
        {
            return ReadAll(fields, FilterExpression.BuildFromJson(this.EntityId, filter));
        }

        public List<int> ReadIds(FilterExpression filter)
        {
            var q = new DbQuery(this);
            q.AddField(this.IDField, true);
            q.SetFixedFilter(filter);
            var data = QueryDbService.ExecuteEntityQuery(q);            
            var list = new List<int>();
            if (data.Count() > 0)
            {
                foreach(var d in data)
                {
                    list.Add(d.Get(this.IDField, 0));
                }
            }

            return list;
        }
        #endregion

    }
}