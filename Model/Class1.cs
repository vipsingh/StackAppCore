using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Model
{
    public class DynamicObj: Dictionary<string, Object>
    {
        public T Get<T>(string attrName, T def)
        {
            if (this.ContainsKey(attrName)) 
            {
                return (T)this[attrName];
            }

            return def;
        }
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
