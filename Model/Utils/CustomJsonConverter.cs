using System;
using Newtonsoft.Json;

namespace StackErp.Model.Utils
{
    public class DynamicObjJsonConverter : JsonConverter<DynamicObj>
{
    public override void WriteJson(JsonWriter writer, DynamicObj value, JsonSerializer serializer)
    {
        value.Serialize(serializer, writer);
    }

    public override DynamicObj ReadJson(JsonReader reader, Type objectType, DynamicObj existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string s = (string)reader.Value;

        return DynamicObj.Parse(s);
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
