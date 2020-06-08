using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackErp.Model;

namespace StackErp.Model.Form
{
    public class EvalParam
    {
        public string Name { set; get; }
        public EvalParamValue Value { set; get; }
        
        public static List<EvalParam> FromJson(string json)
        {
             if (String.IsNullOrWhiteSpace(json))
                return null;

            var pa = new List<EvalParam>();

            var arr = JsonConvert.DeserializeObject(json);
            if (arr is JArray) {
               foreach(JObject jo in (JArray)arr)
               {
                   var p = new EvalParam();
                   p.Name = jo["Name"] == null ? "": jo["Name"].ToString();
                   p.Value = jo["Value"] == null? null: EvalParamValue.FromExp(jo["Value"].ToString());

                   pa.Add(p);
               }
            } 

            return pa;
        }
    }

    public class EvalParamValue
    {
        public EvalSourceType Source { set; get; }
        public object Value { set; get; }

        public static EvalParamValue FromExp(string exp)
        {
            var param = new EvalParamValue() { Source = EvalSourceType.Constant, Value = exp };
            
            if (string.IsNullOrEmpty(exp) || !exp.Trim().StartsWith("@"))
                return param;

            var field = exp.Replace("@","").Trim().ToLower();

            if(exp.Trim().StartsWith("@$"))
            {
                param.Source = EvalSourceType.Env;
                if (field.StartsWith("$qs."))
                    param.Source = EvalSourceType.RequestQuery;
                else if (field.StartsWith("$user."))
                    param.Source = EvalSourceType.User;
                else if (field.StartsWith("$app."))
                    param.Source = EvalSourceType.Env;
                
                field = field.Split('.')[1];
            } 
            else 
            {
                param.Source = EvalSourceType.ModelField;
            }
            param.Value = field;

            return param;
        }
    }

}
