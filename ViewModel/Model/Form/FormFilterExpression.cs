using System;
using System.Collections.Generic;
using StackErp.Model.DataList;
using StackErp.Model.Entity;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.Model
{
    public class FormFilterExpression: Dictionary<int, string[]>
    {
        public static FormFilterExpression Build(FormContext formContext, FilterExpression expression) 
        {
            var exp = new FormFilterExpression();
            int x = 0;
            foreach(var filed in expression.GetAll())
            {
                var schema = formContext.Entity.GetFieldSchema(filed.FieldName);
                if (schema != null)
                    exp.Add(x++, new string[] { schema.ViewName, ((int)filed.Op).ToString(), filed.Value });
            }

            return exp;
        }

        public List<string> GetCriteriaFields()
        {
            var s = new List<string>();
            foreach(var f in this.Values){
                s.Add(f[0]);
            }

            return s;
        }
    }
}