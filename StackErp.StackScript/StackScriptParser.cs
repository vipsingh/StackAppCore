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
            var program = parser.ParseScript();
            
            //StringBuilder sb = new StringBuilder();
            //StringWriter sw = new StringWriter(sb);
            //var w = new JsonTextWriter(sw);

            //program.WriteJson(sw);
            return "";
        }
    }

    public class StackScriptExecuter {
        private Dictionary<string, object> _Vars;
        private ObjectDataProvider _DataProvider;
        private ObjectDataProvider _VarProvider;
        public StackScriptExecuter()
        {
            _Vars = new Dictionary<string, object>();
            //_Vars.Add("coll", new List<object>() { "x", "v", "z" });
            
        }

        public AnyStatus ExecuteScript(string codeStr) 
        {
            AnyStatus sts = AnyStatus.Success;
            try {
                var parser =  new JavaScriptParser(codeStr);
                var program = parser.ParseScript();

                ExecuteScript(program);
            } catch(Exception ex) {
                sts = AnyStatus.ScriptFailure;
                sts.Message = ex.Message;
            }

            return  sts;
        }

        public void ExecuteScript(Script program) 
        {
            _DataProvider = new ObjectDataProvider();
            ExecuteBody(program.Body);
        }

        public object ExecuteExpression(string code, EntityModelBase model)
        {
            _DataProvider = new ObjectDataProvider();
            _VarProvider = new EntityModelDataProvider(model);
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
            _Vars[name] = value;
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
            if (this._VarProvider != null && !_Vars.ContainsKey(idf.Name))
            {
                return this._VarProvider.GetVarData(idf.Name);
            }

            return _Vars[idf.Name];
        }

        private object LteralExp(Literal exp) 
        {
            if (exp.Value is double)
                return Convert.ToDecimal(exp.Value);
            return exp.Value;
        }

        private object BinaryExpression(BinaryExpression expression) {
            var lVal = GetVarFromExp(expression.Left);
            var rVal = GetVarFromExp(expression.Right);
            //BinaryOperator
            return BinaryFunctions.Get(expression.Operator).Invoke(new Arguments(lVal, rVal)); //_ExpFunc[expression.operator]([lVal, rVal]);
        }

        private void AssignmentExpression(AssignmentExpression expression) {
            var lVal = ((Identifier)expression.Left).Name;
            var rVal = GetVarFromExp(expression.Right);
            
            SetVar(lVal, rVal);
        }

        private void IfStatement(IfStatement stmt){
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
            if (expression.Property is Identifier)
            {
                return _DataProvider.GetObjectData(objName, ((Identifier)expression.Property).Name, args);
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
                    IEnumerable<object> collData = null;
                    if (coll is IEnumerable<object>){
                        collData = (IEnumerable<object>)coll;
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