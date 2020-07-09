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

        public static ObjectFunctionScriptInfo GetFunction(Type type, string name)
        {
            Type ty1 = null;
            if (!_data.ContainsKey(type))
            {
                foreach(Type t in _data.Keys)
                {
                    if (type.IsSubclassOf(t))
                    {
                        ty1 = t;
                    }
                }
            }
            else 
                ty1 = type;

            if (ty1 == null)    
                throw new ScriptException($"Function {type.Name}.{name} is not valid.");

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
            var d = new  Dictionary<string, ObjectFunctionScriptInfo>();
            d.Add("GetValue", new ObjectFunctionScriptInfo(typeof(EntityModelBase), "GetValue"));
            d.Add("SetValue", new ObjectFunctionScriptInfo(typeof(EntityModelBase), "SetValue"));
            
            _data.Add(typeof(EntityModelBase), d);
        }

        static void RegisterDbEntity()
        {
            var d = new  Dictionary<string, ObjectFunctionScriptInfo>();
            d.Add("Save", new ObjectFunctionScriptInfo(typeof(IDBEntity), "Save"));            

            _data.Add(typeof(IDBEntity), d);
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
                return Model.GetType().GetMethod(Name).Invoke(Model, arguments.ToArray());
            });
        }
        public ScriptObjectunction Function {set;get;}
        // public List<Type> ParamTypes {set;get;}
        // public Type ReturnType {set;get;}
    }
}