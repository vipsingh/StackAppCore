using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using StackErp.Model;

namespace StackErp.StackScript
{
    internal class ObjectDataProvider
    {
        Dictionary<string, object> _Vars;
        private StackAppContext _appContext;
        internal ObjectDataProvider(StackAppContext appContext)
        {
            _Vars = new Dictionary<string, object>();
            _appContext = appContext;
        }

        internal ObjectDataProvider(StackAppContext appContext, Dictionary<string, object> param): this(appContext)
        {
             foreach(var d in param)
             {
                 _Vars.Add(d.Key, d.Value);
             }
        }

        internal object GetObjectData(string objectName, string propName, Arguments args = null)
        {
            object val = null;
            switch(objectName) {
                case "_":
                    val = GetUtilityResult(propName, args);
                    break;
                case "$app":
                    val = GetAppVariable(propName);
                    break;
                case "$context"://used on server
                    val = GetContextVariable(propName);
                    break;
                // case "model":
                //     val = GetParamData(propName, args);
                //     break;
                default:
                    return ProcessFunction(objectName, propName, args);
                    break;
            }

            return val;
        }

        // internal object GetParamData(string propName, Arguments args = null)
        // {
        //     var d = ScriptObjectTypeFactory.GetFunction<EntityModelBase>(propName);

        //     return d.Function.Invoke(_param1, args);
        // }
        internal object ProcessFunction(string objectName, string propName, Arguments args = null)
        {
            var vr = GetVarData(objectName);
            var d =  ScriptObjectTypeFactory.GetFunction(vr.GetType(), propName);

            return d.Function.Invoke(vr, args);
        }


        internal virtual object GetVarData(string propName)
        {
            return _Vars[propName];
        }

        internal virtual void SetVarData(string name, object value) 
        {
            _Vars[name] = value;
        }

        private object GetUtilityResult(string propName, Arguments args)
        {
            if (UtilityFunctions.ContainsKey(propName))
                return UtilityFunctions.Get(propName).Invoke(args);
            
             throw new ScriptException("Invalid function _." + propName);
        }

        private object GetAppVariable(string propName)
        {           
            //system variable like UserId, userRole, CurrentDate
             throw new ScriptException("Invalid function _." + propName);
        }

        private object GetContextVariable(string propName)
        {           
            //StackAppContext info
             throw new ScriptException("Invalid function _." + propName);
        }
    }

    internal class EntityModelDataProvider: ObjectDataProvider
    {
        EntityModelBase _modelBase;
        internal EntityModelDataProvider(EntityModelBase modelBase): base(null)
        {
            _modelBase = modelBase;
        }
        internal override object GetVarData(string propName)
        {
            return this._modelBase.GetValue(propName);
        }
    }    

}
