using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackErp.Model.DataList;
using StackErp.Model.Entity;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.Model
{
    [JsonConverter(typeof(FilterExpressionJsonConverter))]
    public class FormFilterExpression
    {
        private FilterExpression FilterExp;
        public FormFilterExpression(FormContext formContext, FilterExpression expression)
        {
            FilterExp = expression;
        }

        public IList<string> GetCriteriaFields()
        {
            return this.FilterExp.GetFieldNames();
        }

        public JObject ToJObject()
        {
            return JObject.Parse(this.FilterExp.ToJSONFormat());
        }
    }

    public class FilterExpressionJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FormFilterExpression);
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jo = ((FormFilterExpression)value).ToJObject();
            jo.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return null;
        }
    }
}