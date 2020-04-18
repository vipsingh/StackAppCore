using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackErp.Model;
using StackErp.ViewModel.Model;

namespace StackErp.App.Helpers
{
    public class DynamicObjJsonConverter : JsonConverter
{
     public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DynamicObj) || objectType.IsSubclassOf(typeof(DynamicObj));
    }
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        ((DynamicObj)value).Serialize(serializer, writer);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;
        var token = JToken.Load(reader);
        // if (!(token is JValue))
        //     throw new JsonSerializationException("Token was not a primitive");
        //string s = (string)reader.Value;

        var d = DynamicObj.Parse(token.ToString());
        if (objectType != typeof(DynamicObj)) 
        {
            var i = Activator.CreateInstance(objectType);
            objectType.GetField("_d").SetValue(i, d._d);

            return i;
        }
        return d;
    }
}

public class EntityCodeJsonConverter : JsonConverter<EntityCode>
{
    public override void WriteJson(JsonWriter writer, EntityCode value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Code.ToString());
    }

    public override EntityCode ReadJson(JsonReader reader, Type objectType, EntityCode existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        int s = Int32.Parse(reader.Value.ToString());

        return s;
    }
}
}
