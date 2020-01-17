using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using StackErp.Model;

namespace StackErp.App.Helpers
{
    public class DynamicObjJsonConverter : JsonConverter<DynamicObj>
    {
        public override DynamicObj Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DynamicObj.Parse(reader.GetString());
        }
        public override void Write(Utf8JsonWriter writer, DynamicObj obj, JsonSerializerOptions options)
        {
                writer.WriteStringValue(obj.ToJson());
        }
    }
}
