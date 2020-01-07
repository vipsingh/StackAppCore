using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StackErp.Model
{
    public class DynamicObj
    {
        private Dictionary<string, Object> _d;

        public DynamicObj() {
            _d = new Dictionary<string, object>();
        }
        public Dictionary<string, object>.KeyCollection Keys { get => _d.Keys; }
        public void Add(string key, object value, bool isOverride = false) {
            if(!ContainsKey(key.ToUpper()))
                _d.Add(key.ToUpper(), value);
            else if(isOverride) {
                _d[key] = value;
            }
        }
        public bool ContainsKey(string key) {
            return _d.ContainsKey(key.ToUpper());
        }
        public bool Remove(string key) {
            return _d.Remove(key.ToUpper());
        }
        public void Clear() {
            _d.Clear();
        }
        public T Get<T>(string attrName, T def)
        {
            if (_d.ContainsKey(attrName.ToUpper())) 
            {
                var v = _d[attrName.ToUpper()];

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
    }
}
