using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace StackErp.Model.Utils
{
    public static class JSONUtil
    {
        public static Newtonsoft.Json.JsonSerializerSettings JsonSettingsIncludeNull;
        static Newtonsoft.Json.JsonSerializerSettings JsonSettingsIgnoreNullEmpty;

        static Newtonsoft.Json.JsonSerializerSettings PrivateSetter;
        public static JsonSerializerSettings JsonCustomSettings = null;

        static JSONUtil()
        {
            JsonSettingsIncludeNull = new Newtonsoft.Json.JsonSerializerSettings()
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
                    ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace,
                    MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
                    DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include,
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize,
                    Error = (serializer, args) => args.ErrorContext.Handled = true,
                    StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeHtml,
                  MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
                };


            JsonSettingsIgnoreNullEmpty = new Newtonsoft.Json.JsonSerializerSettings()
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace,
                    MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
                    DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include,
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize,
                    Error = (serializer, args) => args.ErrorContext.Handled = true,
                    StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeHtml
                };
             PrivateSetter =  new JsonSerializerSettings() { ContractResolver = new PrivateContractResolver() };


       JsonCustomSettings =   new JsonSerializerSettings
          {
              ContractResolver = new DynamicContractResolver<JsonSerialize>(),
              StringEscapeHandling = StringEscapeHandling.EscapeHtml,
              NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
              
          };
        }
        public static string SerializeObject(object model)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(model, JSONUtil.JsonSettingsIncludeNull);
        }

        /// <summary>
        /// This method, return the json only of those fields/Properties that has the attribute JsonCustomSerialize
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string SerializeCustomizeAttribute(object model)
        {
            return JsonConvert.SerializeObject(model, Formatting.None, JsonCustomSettings);
        }

        public static string SerializeObjectIExcludeNull(object model)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(model, JSONUtil.JsonSettingsIgnoreNullEmpty);
        }

        public static T DeserializeWithPrivateSetter<T>(string json)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, PrivateSetter);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "Invalid Json to deserialze in :" + typeof(T).ToString() + json);
            }
        }

        public static T DeserializeObject<T>(string json)
        {
            try
            {

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message + "Invalid Json to deserialze in :" + typeof(T).ToString() + json);
            }
        }

        // public static string JavaScriptSerializer(object o)
        // {
        //     return new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue }.Serialize(o);
        // }
    }

    public class PrivateContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {     
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(m => m.SetMethod!=null)
                            .Select(p => base.CreateProperty(p, memberSerialization))
                        .Union(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                   .Select(f => base.CreateProperty(f, memberSerialization)))
                        .ToList();

            props.ForEach(p => { p.Writable = true; p.Readable = true; });
            return props;
        }

        
    }

    public class JsonSerialize : Attribute
    { 
    }

    public class DynamicContractResolver<T> : DefaultContractResolver
    {
        Type _AttributeToSerialize = null;
        public DynamicContractResolver()
        {
            _AttributeToSerialize = typeof(T);
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var propsToSerialize = type.GetProperties()
                                .Where(p => p.GetCustomAttributes(true).Any(a => a.GetType() == typeof(T)))
                                .ToDictionary(p => p.Name);

            var list = base.CreateProperties(type, memberSerialization)
                           .Where(p => propsToSerialize.ContainsKey(p.PropertyName))
                           .ToList();

            return list;
        }
    }
}