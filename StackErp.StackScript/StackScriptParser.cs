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

namespace StackErp.StackScript
{    
    public class StackScriptParser
    {
        public static string Parse(string code) 
        {
            var parser =  new JavaScriptParser(code);
            var program = parser.ParseScript();
            var exec =  new StackScriptExecuter(program);
            

            //StringBuilder sb = new StringBuilder();
            //StringWriter sw = new StringWriter(sb);
            //var w = new JsonTextWriter(sw);

            //program.WriteJson(sw);
            return "";
        }
    }

    public class StackScriptExecuter {
        public Dictionary<string, object> _Vars;
        public StackScriptExecuter(Script program)
        {
            _Vars = new Dictionary<string, object>();
            //_Vars.Add("coll", new List<object>() { "x", "v", "z" });
            ExecuteBody(program.Body);
        }

        public void ExecuteBody(NodeList<IStatementListItem> bodyColl) 
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
            if (exp is Literal) {
                return LteralExp(exp as Literal);
            }
            else if (exp is Identifier) {
              return GetVar(exp as Identifier);
            }
            else if (exp is BinaryExpression) {
                 return BinaryExpression(exp as BinaryExpression);
            }
            else if (exp is CallExpression) {
               return CallExpression(exp as CallExpression);
            }

            return null;
        }

        private object GetVar(Identifier idf) {
            return _Vars[idf.Name];
        }

        private object LteralExp(Literal exp) 
        {
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
            var fun = ((Identifier)expression.Callee).Name;
            if (fun == "foreach") {
                ProcessForeach(expression.Arguments);
                return null;
            }
            else if (ScriptFunctions.ContainsKey(fun)) {
                var args = new Arguments();
                foreach(var e in expression.Arguments) {                    
                    args.Add(GetVarFromExp(e as Expression));
                }
                try {
                    return ScriptFunctions.Get(fun).Invoke(args);
                } catch(Exception ex) {
                    throw new ScriptException($"Function '{fun}' error:: {ex.Message}");
                }

            } else {
                throw new ScriptException($"Function with name {fun} is not defined");
            }
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
            } catch(Exception ex) {
                throw;
            }
        }
    }        
}

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