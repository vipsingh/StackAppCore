using System;
using System.Linq;
using StackErp.Model;
using StackErp.Model.Entity;
using StackErp.Model.Form;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel
{
    public class ValueResolver
    {
        public static void ResolveFilterExpressionValue(ref FilterExpression expression, FormContext formContext)
        {
            if (expression != null)
            {
                // get linkedvalue from customrequest
                var entFieldFilters = expression.GetAll().Where(x => !x.IsValueResolved);
                foreach(var filter in entFieldFilters)
                {
                    var val = ResolveValue(formContext, EvalParamValue.FromExp(filter.Exp));
                    filter.SetResolvedValue(val);
                }
            }
        }
        
        public static object ResolveValue(FormContext formContext, EvalParamValue param)
        {
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
            else if (param.Source == EvalSourceType.Parameters)
            {
                return GetParametersValue(formContext, param.Value.ToString());
            }

            return param.Value;
        }

        private static object GetFieldValue(FormContext formContext, string field)
        {
            object v = null;
            if(formContext.EntityModel != null)
            {
                v = formContext.EntityModel.GetValue(field);                
            }
            else
            {
                var linkedData = formContext.GetParameter<DynamicObj>(ViewConstant.LinkedData, null);
                if (linkedData != null)
                {
                    v = linkedData.Get<object>(field, null);
                }   
            }

            if(v is SelectOption)
                return ((SelectOption)v).Value;
            return v;
        }

        private static object GetRequestQueryValue(RequestQueryString requestQuery, string key)
        {
            return requestQuery.GetData(key);
        }
        
        private static object GetParametersValue(FormContext formContext, string key)
        {
            if(formContext.EntityModelInfo != null)
            {
               var param = formContext.EntityModelInfo.Get<DynamicObj>("Parameters", null);

                return param.Get<object>(key, null);
            }

            return null;
        }
    }
}
