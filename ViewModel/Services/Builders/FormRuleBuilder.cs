using System;
using System.Collections.Generic;
using StackErp.Model.DataList;
using StackErp.Model.Entity;
using StackErp.Model.Layout;
using StackErp.Model.Localization;
using StackErp.ViewModel.Model;
using StackErp.ViewModel.ViewContext;

namespace StackErp.ViewModel.Services
{
    public class FormRuleBuilder
    {
        public static List<FormRule> BuildRules(FormContext formContext, TView layoutView)
        {
            var formR = new List<FormRule>();

            if (layoutView.FormRules != null)
            {
                int i = 0;
                foreach(var tRule in layoutView.FormRules)
                {
                    var r = AddRule(tRule, formContext);
                    r.Id = i++;
                    if(r!= null)
                     formR.Add(r);
                }
            }

            return formR;
        }

        private static FormRule AddRule(TFormRule tRule, FormContext formContext)
        {
            if(!String.IsNullOrEmpty(tRule.Criteria))
            {
                var rule = new FormRule();
                var expression = FilterExpression.BuildFromJson(formContext.Entity.EntityId, tRule.Criteria);
                rule.Criteria = FormFilterExpression.Build(formContext, expression);
                rule.Type = tRule.Type;
                rule.Fields = new List<string>();
                foreach(string f in tRule.Fields) {
                    var schema = formContext.Entity.GetFieldSchema(f);
                    rule.Fields.Add(schema.ViewName);
                }

                formContext.MissingFields.AddRange(expression.GetFieldNames());

                return rule;                
            }

            return null;
        }
    }
}