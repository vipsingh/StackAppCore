using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StackErp.Model.Entity
{
    public class FilterExpression
    {
        private List<FilterExpField> _data;
        public EntityCode EntityId {get;}
        
        public FilterGroup FilterGroup { get; set; }
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

        public string ToJSONFormat() 
        {
            var jo = new JObject();
            var filterArr = new JArray();
            foreach(var field in this._data) {
                filterArr.Add(field.ToJSONObj());
            }
            jo.Add(this.FilterGroup == FilterGroup.OR? "$or": "$and", filterArr);

            return jo.ToString();
        }

        public static FilterExpression BuildFromJson(EntityCode entity, string json) 
        {
            if (String.IsNullOrWhiteSpace(json))
                return null;
            var exp = new FilterExpression(entity);

            var arr = JsonConvert.DeserializeObject(json);
            string groupOp = "$and";
            if (arr is JObject) {
                if (((JObject)arr).Count > 0) {
                    var groupElm = ((JObject)arr).Properties().First();
                    groupOp = groupElm.Name;
                    var filters = (JArray)groupElm.Value;
                    ProcessFilterJson(filters, ref exp);
                }
            } else if(arr is JArray) {
                ProcessFilterJson((JArray)arr, ref exp);
            }
            exp.FilterGroup = groupOp == "$or"? FilterGroup.OR: FilterGroup.AND;

            return exp;
        }

        private static void ProcessFilterJson(JArray filters, ref FilterExpression exp) {
            foreach(JObject obj in filters) {
                exp.Add(FilterExpField.BuildFromJson(obj));
            }
        }
    }

    public class FilterExpField
    {
        public string FieldName {get;}
        public string Value {get;}
        public BaseField Field {get;}
        public FilterOperationType Op {get;}
        public FilterValueType ValueType {get;}

        public Int16 SeqId {set;get;}
        internal bool IsValueResolved = true;
        public  FilterExpField(string field, FilterOperationType op, string value) {
            FieldName = field;
            Op=op;
            Value = value;
            ValueType = FilterValueType.Value;

            if (!String.IsNullOrWhiteSpace(value) && value.StartsWith("${")) 
            {
                this.IsValueResolved = false;
                ValueType = FilterValueType.EntityField;
                if (value.Contains("stack.")) 
                {
                    ValueType = FilterValueType.AppField;
                }
                Value = value.Replace("${", "").Replace("}", "").Trim();
            }
        }

        internal JObject ToJSONObj()
        {
            var jo = new JObject();
            var arr = new JArray();
            arr.Add((int)this.Op);
            arr.Add(this.Value);
            jo.Add(this.FieldName, arr);

            return jo;
        }

        public static object[] GetValueSet(String val, Type type)
        {
            ArrayList list = new ArrayList();
            String[] vals = val.Split(',');

            foreach (String s in vals)
            {
                if (s.Trim().Length > 0)
                {
                    list.Add(Convert.ChangeType(s.Trim(), type));
                }
            }

            return (object[])list.ToArray(typeof(object));
        }

        internal static FilterExpField BuildFromJson(JObject json)
        {
             if (json.Count > 0) {
                    var fieldElm = json.Properties().First();
                    var field = fieldElm.Name;
                    var data = (JArray)fieldElm.Value;
                    var v = new FilterExpField(field, (FilterOperationType)((int)data[0]),data[1].ToString());

                    return v;
                }
            return null;
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
    Between = 16,
    FixedExpression = 17, //expression is given like xy < 45
    Expression = 18,
    AnyOf = 19
    }

    public enum FilterValueType 
    {
        Value = 0,
        EntityField = 1,
        AppField = 2
    }

    public enum FilterGroup 
    {
        AND = 0,
        OR = 1
    }
}

/*
Format 1
  {
    "$or": [
      {
        "status": [0, "A"]
      },
      {
        "qty": [2, 30]
      },
      {
        "role.name": [0, "admin"]
      }
    ]
  }

  Format 2
    [{},,] default and

*/