using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StackErp.Model
{
    [JsonConverter(typeof(StackErp.Model.Utils.DynamicObjJsonConverter))]
    public class DynamicObj
    {
        private Dictionary<string, Object> _d;

        public DynamicObj() {
            _d = new Dictionary<string, object>();
        }
        private DynamicObj(Dictionary<string, object> d)
        {
            _d = d;
        }
        public Dictionary<string, object>.KeyCollection Keys { get => _d.Keys; }
        public void Add(string key, object value, bool isOverride = false) {
            if(!ContainsKey(key))
                _d.Add(key, value);
            else if(isOverride) {
                _d[key] = value;
            }
        }
        public bool ContainsKey(string key) {
            return _d.ContainsKey(key);
        }
        public bool Remove(string key) {
            return _d.Remove(key);
        }
        public void Clear() {
            _d.Clear();
        }
        public T Get<T>(string attrName, T def)
        {
            if (_d.ContainsKey(attrName)) 
            {
                var v = _d[attrName];

                return DataHelper.GetData(v, def);
            }

            return def;
        }
        public Object this[string key] { get => this.Get<object>(key, null); }
        public static DynamicObj From(IDictionary<string, Object> data) {
            var d = new DynamicObj();
            foreach(var k in data) {
                d.Add(k.Key, k.Value);
            }

            return d;
        }

        public static DynamicObj FromJSON(string json) 
        {
            var obj = (JObject)JsonConvert.DeserializeObject(json);
            if(obj == null)
                return null;
                
            var d = new DynamicObj();
            foreach(var k in obj) {
                var v = obj[k];
                if (v is JObject) 
                {
                    d.Add(k.Key, DynamicObj.FromJSON(v.ToString()));
                } else {
                    d.Add(k.Key, v);
                }
            }

            return d;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this._d);
        }

        public static DynamicObj Parse(string json)
        {
            var d = JsonConvert.DeserializeObject<Dictionary<string, Object>>(json);
            
            return new DynamicObj(d);
        }

        public static DynamicObj DeserializeJObject(object jObject)
        {
            if (jObject == null)
                return null;

            var row = jObject as Dictionary<string,object>;

            if (row != null)
                return new DynamicObj(row);

            if (jObject is JObject || jObject is string)
            {
                return DynamicObj.Parse(jObject.ToString());
            }


            return null;
        }
    }

    public class SelectOption: DynamicObj
    {
        public int Value {get => this.Get("Value", 0);}
        public string Text {get => this.Get("Text", "");}
        public string Code {get => this.Get("Code", "");}
    }
}
