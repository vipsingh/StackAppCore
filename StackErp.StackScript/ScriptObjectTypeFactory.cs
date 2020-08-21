using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.StackScript
{
    public delegate object ScriptObjectunction(object model, Arguments arguments);
    public class ScriptObjectTypeFactory
    {
        private static Dictionary<Type, Dictionary<string, ObjectFunctionScriptInfo>> _data;
        static ScriptObjectTypeFactory()
        {
            _data = new Dictionary<Type, Dictionary<string, ObjectFunctionScriptInfo>>();
            RegisterEntityModelBase();
            RegisterDbEntity();
        }        

        public static void Register(Type type, Dictionary<string, ObjectFunctionScriptInfo> functions)
        {
            _data.Add(type, functions);
        }

        public static void Register(Type type, params string[] funcs)
        {
            var d = new  Dictionary<string, ObjectFunctionScriptInfo>();
            foreach(var f in funcs)
            {
                d.Add(f, new ObjectFunctionScriptInfo(type, f));
            }
            _data.Add(type, d);
        }

        public static ObjectFunctionScriptInfo GetFunction(Type type, string name)
        {
            Type ty1 = null;
            if (!_data.ContainsKey(type))
            {
                foreach(Type t in _data.Keys)
                {
                    if (t.IsInterface && t.IsAssignableFrom(type)) 
                    {
                        ty1 = t;
                        break;
                    }
                    else if (type.IsSubclassOf(t))
                    {
                        ty1 = t;
                        break;
                    }
                }
            }
            else 
                ty1 = type;

            if (ty1 == null)    
                throw new ScriptException($"Object at {type.Name}.{name} is not valid.");

            var dic = _data[ty1];
            
            if (!dic.ContainsKey(name))
                throw new ScriptException($"Function {type.Name}.{name} is not valid function");

            return _data[ty1][name];
        }

        public static ObjectFunctionScriptInfo GetFunction<T>(string name)
        {
            return GetFunction(typeof(T), name);
        }

        static void RegisterEntityModelBase()
        {
            Register(typeof(EntityModelBase), "GetValue", "SetValue");
            Register(typeof(DBModelBase), "GetValue");
        }

        static void RegisterDbEntity()
        {
            Register(typeof(IDBEntity), "Save", "GetDefault", "GetSingle", "ReadIds", "Read", "ReadAll");
        } 

        static void RegisterCommon()
        {
            Register(typeof(System.Collections.IList), "Add", "Contains", "IndexOf", "Remove", "RemoveAt");
        }        
    }

    public class ObjectFunctionScriptInfo
    {
        public List<MemberInfo> Methods;
        public string Name;
        private bool isInterFaceType = false;
        public ObjectFunctionScriptInfo(Type type1, string methodName)
        {
            if (type1.IsInterface)
            {                
                isInterFaceType = true;
            }
            
            Methods = type1.GetMembers(BindingFlags.Public).Where(x => x.Name == methodName).ToList();

            Name = methodName;

            Function = new ScriptObjectunction((Model, arguments) =>
            {  
                var method = Model.GetType().GetMethod(Name, arguments.ToTypeArray());
                if (method == null) throw new ScriptException($"No method found with name: {Name} and parameters");

                return method.Invoke(Model, arguments.ToArray());
            });
        }
        public ScriptObjectunction Function {set;get;}
        // public List<Type> ParamTypes {set;get;}
        // public Type ReturnType {set;get;}

    }
}