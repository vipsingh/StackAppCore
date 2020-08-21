using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Esprima;
using Esprima.Ast;
using Esprima.Utils;
using Newtonsoft.Json;
using System.Globalization;
using StackErp.Model;

namespace StackErp.StackScript
{    
    public class StackScriptParser
    {
        public static string Parse(string code) 
        {
            var parser =  new JavaScriptParser(code);
            var program = parser.ParseModule();
            
            //StringBuilder sb = new StringBuilder();
            //StringWriter sw = new StringWriter(sb);
            //var w = new JsonTextWriter(sw);

            //program.WriteJson(sw);
            return "";
        }
    }

    public class StackScriptExecuter {
        private ObjectDataProvider _DataProvider;
        private string currentScriptTag = "";
        public StackScriptExecuter()
        {
            
        }

        public AnyStatus ExecuteScript(string codeStr) 
        {
            currentScriptTag = "Start<>";
            AnyStatus sts = AnyStatus.Success;
            try {
                var parser =  new JavaScriptParser(codeStr);
                var program = parser.ParseScript();

                ExecuteScript(program);
            } catch(Exception ex) {
                sts = AnyStatus.ScriptFailure;
                sts.Message = currentScriptTag + "::" + ex.Message;
            }

            return  sts;
        }

        public AnyStatus ExecuteFunction<T>(StackAppContext appContext, string codeStr, Dictionary<string, object> param, T output) //Arguments param
        {
            AnyStatus sts = AnyStatus.Success;            
            try {
                currentScriptTag = "Start<>";

                var parser =  new JavaScriptParser(codeStr);
                var program = parser.ParseScript();

                _DataProvider = new ObjectDataProvider(appContext, param);
                _DataProvider.output = output;

                ExecuteBody(program.Body);

            } catch(Exception ex) {
                sts = AnyStatus.ScriptFailure;
                sts.Message = currentScriptTag + "::" + ex.Message;
            }

            return  sts;
        }

        public void ExecuteScript(Script program) 
        {
            _DataProvider = new ObjectDataProvider(null);
            ExecuteBody(program.Body);
        }

        public object ExecuteExpression(string code, EntityModelBase model)
        {
            _DataProvider = new EntityModelDataProvider(model);
            var parser =  new JavaScriptParser(code);
            var expression = parser.ParseExpression();

            return GetVarFromExp(expression);
        }

        private void ExecuteBody(NodeList<IStatementListItem> bodyColl) 
        {            
            foreach(IStatementListItem body in bodyColl)
            {                
                if (body is Esprima.Ast.VariableDeclaration) {
                    VariableDeclaration(((VariableDeclaration)body).Declarations);
                }
                else if(body is ExpressionStatement) {
                    ExpressionStatement(((ExpressionStatement)body).Expression);
                }
                else if(body is IfStatement) {
                    IfStatement(body as IfStatement);
                }                  
            }
        }

        private void VariableDeclaration(NodeList<VariableDeclarator> declartions) 
        {
            var dec = declartions.First();
            if (dec.Id is Esprima.Ast.Identifier) {                
                var varNme= ((Esprima.Ast.Identifier)dec.Id).Name;
                currentScriptTag = $"variable: {varNme}";

                var varVal = GetVarFromExp(dec.Init);

                SetVar(varNme, varVal);
            }
        }

        private void ExpressionStatement(Expression exp) {
            if (exp is AssignmentExpression) {
                AssignmentExpression(exp as AssignmentExpression);
            } else if (exp is CallExpression) {
                CallExpression(exp as CallExpression);
            }
        }

        private void SetVar(string name, object value) 
        {
             currentScriptTag = $"set variable value: {name}";
            _DataProvider.SetVarData(name, value);
        }

        private object GetVarFromExp(Expression exp) {
            if (exp.Type == Nodes.Literal) {
                return LteralExp(exp as Literal);
            }
            else if (exp.Type == Nodes.Identifier) {
              return GetVar(exp as Identifier);
            }
            else if (exp.Type == Nodes.BinaryExpression || exp.Type == Nodes.LogicalExpression) {
                 return BinaryExpression(exp as BinaryExpression);
            }
            else if (exp.Type == Nodes.CallExpression) {
               return CallExpression(exp as CallExpression);
            }
            else if (exp.Type == Nodes.MemberExpression) {
               return MemberExpression(exp as MemberExpression);
            }
            else
                throw new ScriptParsingException("Expression not allowed: " + exp.Type.ToString());

        }

        private object GetVar(Identifier idf) {
            return this._DataProvider.GetVarData(idf.Name);
        }

        private object LteralExp(Literal exp) 
        {
            if (exp.Value is double)
            {
                if (exp.Raw.Contains("."))
                    return Convert.ToDecimal(exp.Value);
                else 
                    return Convert.ToInt32(exp.Value);
            }
            return exp.Value;
        }

        private object BinaryExpression(BinaryExpression expression) {
            var lVal = GetVarFromExp(expression.Left);
            var rVal = GetVarFromExp(expression.Right);

            currentScriptTag = $"BinaryExpression: {expression.Operator} Left: {lVal}";
            //BinaryOperator
            return BinaryFunctions.Get(expression.Operator).Invoke(new Arguments(lVal, rVal)); //_ExpFunc[expression.operator]([lVal, rVal]);
        }

        private void AssignmentExpression(AssignmentExpression expression) {
            var rVal = GetVarFromExp(expression.Right);

            currentScriptTag = $"Assignment";

            if (expression.Left is MemberExpression) 
            {
                SetMemberExpression(expression.Left as MemberExpression, rVal);
            }
            else 
            {
                var lVal = ((Identifier)expression.Left).Name;                        
                SetVar(lVal, rVal);
            }
        }

        private void SetMemberExpression(MemberExpression expression, object val)
        {
            var objName = ((Identifier)expression.Object).Name;
            if (expression.Property is Identifier)
            {
                _DataProvider.SetObjectData(objName, ((Identifier)expression.Property).Name, val);
            }
        }

        private void IfStatement(IfStatement stmt){
            currentScriptTag = $"IfStatement";

            var test = BinaryExpression(stmt.Test as BinaryExpression);
    
            if (test != null && test is bool && (bool)test) {
                var block = stmt.Consequent as BlockStatement;
                ExecuteBody(block.Body);
            } else {
                var block = stmt.Alternate as BlockStatement;
                ExecuteBody(block.Body);
            }
        }
        private object CallExpression(CallExpression expression) {
            if (expression.Callee is Identifier)
            {
                var fun = ((Identifier)expression.Callee).Name;
                if (fun == "foreach") {
                    ProcessForeach(expression.Arguments);
                    return null;
                }
                else if (ScriptFunctions.ContainsKey(fun)) {
                    try {
                        return ScriptFunctions.Get(fun).Invoke(GetArguments(expression.Arguments));
                    } catch(Exception ex) {
                        throw new ScriptException($"Function '{fun}' error:: {ex.Message}");
                    }

                } else {
                    throw new ScriptException($"Function with name {fun} is not defined");
                }
            } else if (expression.Callee is MemberExpression) {
                return MemberExpression(expression.Callee as MemberExpression, GetArguments(expression.Arguments));
            }
            else {
                throw new ScriptException($"Expression with name {expression.Type.ToString()} is not allowed");
            }
        }

        private Arguments GetArguments(NodeList<ArgumentListElement> args)
        {
            var l = new Arguments();
            foreach(var e in args) {                    
                l.Add(GetVarFromExp(e as Expression));
            }
            return l;
        }

        private object MemberExpression(MemberExpression expression, Arguments args = null)
        {
            var objName = ((Identifier)expression.Object).Name;
            string propName = ((Identifier)expression.Property).Name;
            
            currentScriptTag = $"object member {objName}.{propName}";

            if (expression.Property is Identifier)
            {
                if (args == null) 
                {
                    return _DataProvider.GetObjectPropData(objName, propName);
                }
                else
                {                    
                    return _DataProvider.GetObjectFunctionData(objName, propName, args);
                }
            }

            return null;
        }

        private void ProcessForeach(NodeList<ArgumentListElement> args)
        {
            try {
                if (args.Count != 2) {
                    throw new ScriptException("Invalid foreach statement");
                }

                var f = args.First();
                if (f is Identifier) {
                    var coll = GetVarFromExp(f as Identifier);
                    IList collData = null;
                    if (coll is IList){
                        collData = (IList)coll;
                    } else throw new ScriptException("Invalid foreach statement");

                    var exp = args[1];
                    if (exp is ArrowFunctionExpression) {
                        var expArrow = (ArrowFunctionExpression)exp;
                        var paramName = ((Identifier)expArrow.Params[0]).Name;
                        foreach(var d in collData)
                        {
                            SetVar(paramName, d);
                            var block = expArrow.Body as BlockStatement;
                            ExecuteBody(block.Body);
                        }
                    } else {
                        throw new ScriptException("Invalid foreach statement");
                    }

                } else {
                    throw new ScriptException("Invalid foreach statement");
                }
            } catch(Exception) {
                throw;
            }
        }
    }        
}
/*Example
var coll = Collection('v', 'f', 'bb');
foreach(coll, (v)=>{
	log(v);
});

*/
/* 
SystemVariable-----------------
$app.  userid, approot
Conditional Expressions--------
var x = iftrue(z == 1, 2, 4);
ifnull(input.number_field,0)
contains

Array----------
Collection(,,)  -> .add, .get -> List<Object>
Object({}) -> get, set  -> DynamicObj in App

DateTime functions -- user reflection(or dynamic) to call DateTime builtin func
    now(), isDate()
CONCAT, TRIM
parse('Int', val) like Int.Parse()

Allowed C# functions
Convert.
Math.

*/