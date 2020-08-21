using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StackErp.Model
{
    public class DbObject
    {
        private InvariantDictionary<Object> _d;

        public DbObject() {
            _d = new InvariantDictionary<object>();
        }
        private DbObject(InvariantDictionary<object> d)
        {
            _d = d;
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
        public static DbObject From(IDictionary<string, Object> data) {
            var d = new DbObject();
            foreach(var k in data) {
                d.Add(k.Key, k.Value);
            }

            return d;
        }

        public static DbObject FromJSON(string json) 
        {
            var obj = (JObject)JsonConvert.DeserializeObject(json);
            if(obj == null)
                return null;
                
            var d = new DbObject();
            foreach(var k in obj) {
                var v = k.Value;
                if (v is JObject) 
                {
                    d.Add(k.Key, DbObject.FromJSON(v.ToString()));
                } else {
                    d.Add(k.Key, v);
                }
            }

            return d;
        }
        
    }

    public class DbTable: List<DbObject>
    {
        
    }
}
