using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StackErp.Model.DataList
{
    public class FilterExpression
    {
        private List<FilterExpField> _data;
        public EntityCode EntityId {get;}
        public FilterExpression(EntityCode entityid) {
            EntityId = entityid;
            _data = new List<FilterExpField>();
        }

        public void Add(FilterExpField field) {
            this._data.Add(field);
        }

        public IList<string> GetFieldNames() {
            return this._data.Select(x => x.FieldName).ToList();
        }

        public List<FilterExpField> GetAll()
        {
            return _data;
        }

        public static FilterExpression BuildFromSimpleJson(EntityCode entity, string json) 
        {
            //ex1: [["F1", 0, "VV"],[,,]]
            if (String.IsNullOrWhiteSpace(json))
                return null;
            var exp = new FilterExpression(entity);

            var arr = (JArray)JsonConvert.DeserializeObject(json);
            if (arr != null) {
                
            }

            return exp;
        }
    }

    public class FilterExpField
    {
        public string FieldName {get;}
        public string Value {get;}
        public FilterOperationType Op {get;}

        public Int16 SeqId {set;get;}
        internal bool IsValueResolved = true;
        public  FilterExpField(string field, FilterOperationType op, string value) {
            FieldName = field;
            Op=op;
            Value = value;

            if (!String.IsNullOrWhiteSpace(value) && value.StartsWith("@"))
                this.IsValueResolved = false;
        }
    }

    public enum FilterOperationType 
    {
    Equal = 0,
    NotEqual = 1,
    GreaterThan = 3,
    LessThan = 4,
    GreaterThanEqual = 5,
    LessThanEqual = 6,
    In = 7,
    NotIn = 8,
    Contains = 9,
    StartWith = 10,
    EndWith = 11,
    Like = 12,
    NotLike= 13,
    IsSpecified = 14,
    NotSpecified = 15,
    Between = 16
    }
}

/*
where: {
    name: 'a project',
    [Op.or]: [
      { id: [1,2,3] },
      { id: { [Op.gt]: 10 } }
    ]
  }
*/