using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using StackErp.Model;

namespace StackErp.Model.Form
{
    public abstract class UIExpression
    {
        protected string exp = null;

        public UIExpression(string exp)
        {
            Exp = exp;
        }

        public string Exp
        {
            get
            {
                return exp;
            }
            private set
            {
                exp = value;
                ReadKeyWords();
            }
        }

        [JsonIgnore]
        public List<string> KeyWords
        {
            get;
            protected set;
        }
        [JsonIgnore]
        public bool IsValid { get; protected set; }
        protected void ReadKeyWords()
        {
            if (string.IsNullOrWhiteSpace(exp))
                return;

            KeyWords = GetExpressionFields(exp);

            if (KeyWords != null && KeyWords.Count > 0)
                IsValid = true;
        }

        public abstract List<string> GetExpressionFields(string expression);
        
    }

    public class TextExpression: UIExpression
    {
        public TextExpression(string exp): base(exp)
        {

        }        
        
        public override List<string> GetExpressionFields(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return null;            

            string pattern = @"(\${[\w_]*})"; // hi hello ${username}.
            List<string> fields = new List<string>();
            foreach (Match m in Regex.Matches(expression, pattern))
            {
                fields.Add(m.ToString().Replace("${", "").Replace("}", ""));
            }
            return fields;
        }   

        public string ResolveDisplayExp(EntityModelBase modelBase)
        {
            if (!IsValid || modelBase == null)
            {
                return exp;
            }

            string expression = exp;

            foreach (var field in KeyWords)
            {
                string displayText = "";
                var result = modelBase.GetValueData(field);

                if (!result.IsValid)
                {
                    displayText = "";
                }
                else if (string.IsNullOrWhiteSpace(result.DisplayValue))
                {
                    displayText = result.Value.ToString();
                }

                expression = RepacleField(expression, field, displayText);
            }

            //ReplaceGlobalFields()

            expression = expression.Replace("  ", " ");

            return expression;
        }

        protected string RepacleField(string expression, string field, string data)
        {
            return expression.Replace("${" + field + "}", data);
        }        
    }

    public class EvalExpression: UIExpression
    {
        [JsonIgnore]
        public List<(string, int)> KeyWordsWithIndex
        {
            get;
            protected set;
        }
        public EvalExpression(string exp): base(exp)
        {

        }
        
        public override List<string> GetExpressionFields(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return null;

            string str = expression.Replace("(","( ").Replace("+"," + ");

            string pattern = @"[\$+a-zA-Z+\(|\']+"; // .
            //List<string> fields = new List<string>();
            var fWithIndex = new List<(string, int)>();
            foreach (Match m in Regex.Matches(expression, pattern))
            {
                var s = m.ToString();
                if (!s.StartsWith("(") && !s.EndsWith("(") && !s.EndsWith("'") && !s.EndsWith("\"") && !(new List<string>(){ "true", "false","+" }).Contains(s.ToLower()))
                {                    
                    fWithIndex.Add((m.ToString(), m.Index));
                }
            }
            return fWithIndex.Select(X => X.Item1).ToList();
        }

        public string ResolveDisplayExp(EntityModelBase modelBase)
        {
            if (!IsValid || modelBase == null)
            {
                return exp;
            }

            string expression = exp;

            foreach (var field in this.KeyWordsWithIndex)
            {
                string replaceVal = "";
                var result = modelBase.GetValueData(field.Item1);

                if (result.IsValid && result.Value != null)
                {
                    replaceVal = result.Value.ToString();
                }
                                
                //aStringBuilder.Remove(3, 2);
                //aStringBuilder.Insert(3, "ZX");
                //theString = aStringBuilder.ToString();

                //expression = expression. ("${" + field + "}", replaceVal);                
            }

            //ReplaceGlobalFields()

            expression = expression.Replace("  ", " ");

            return expression;
        }
    }

    public class FieldPathExpression: UIExpression
    {
        public string LinkField {private set; get;}
        public string Field {private set; get;}
        public FieldPathExpression(string exp): base(exp)
        {

        }  

        public override List<string> GetExpressionFields(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return null;            

            var farr = expression.Split('.');

            LinkField = farr[0];
            Field = farr[1];

            return farr.ToList();
        } 
    }
}