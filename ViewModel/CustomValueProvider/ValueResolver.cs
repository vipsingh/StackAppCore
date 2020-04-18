using System;
using StackErp.Model;
using StackErp.Model.Form;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel
{
    public class ValueResolver
    {
        public static object ResolveValue(FormContext formContext, EvalParam evalParam)
        {
            var param = evalParam.Value;
            if(param.Source == EvalSourceType.Constant) 
            {
                return param.Value;
            }
            else if (param.Source == EvalSourceType.ModelField)
            {
                return GetFieldValue(formContext, param.Value.ToString());
            }
            else if (param.Source == EvalSourceType.RequestQuery)
            {
                return GetRequestQueryValue(formContext.RequestQuery, param.Value.ToString());
            }

            return param.Value;
        }

        private static object GetFieldValue(FormContext formContext, string field)
        {
            if(formContext.EntityModel != null)
            {
                var v = formContext.EntityModel.GetValue(field);
                if(v is SelectOption)
                    return ((SelectOption)v).Value;
                return v;
            }

            return null;
        }

        private static object GetRequestQueryValue(RequestQueryString requestQuery, string key)
        {
            return requestQuery.GetData(key);
        }
    }
}
