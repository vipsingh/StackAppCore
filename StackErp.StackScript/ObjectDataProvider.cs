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
        internal object output;
        private StackAppContext _appContext;
        internal ObjectDataProvider(StackAppContext appContext)
        {
            _Vars = new Dictionary<string, object>();
            _appContext = appContext;
            output = null;
        }

        internal ObjectDataProvider(StackAppContext appContext, Dictionary<string, object> param): this(appContext)
        {
             foreach(var d in param)
             {
                 _Vars.Add(d.Key, d.Value);
             }
        }

        internal object GetObjectFunctionData(string objectName, string propName, Arguments args)
        {
            object val = null;
            switch(objectName) {
                case "_":
                    val = GetUtilityResult(propName, args);
                    break;
                default:
                    return ProcessFunction(objectName, propName, args);
                    break;
            }

            return val;
        }

        internal object GetObjectPropData(string objectName, string propName)
        {
            object val = null;

            switch(objectName) 
            {                
                case "$app":
                    val = GetAppVariable(propName);
                    break;
                case "$context"://used on server
                    val = GetContextVariable(propName);
                    break;
                default:
                    return ProcessObjectProp(objectName, propName);
                    break;
            }

            return val;
        }

        internal void SetObjectData(string objectName, string propName, object value)
        {
            var model = GetVarData(objectName);
            if (model != null) 
            {
                if (model is DynamicObj) 
                {
                    ((DynamicObj)model).Add(propName, value, true);
                    return;
                }

                var type = model.GetType();                
                var prop =  type.GetProperty(propName);
                if (prop != null)
                {                    
                    prop.SetValue(model, DataHelper.GetDataValue(value, prop.PropertyType));
                }
                else 
                {
                    throw new ScriptException($"Object at {objectName}.{propName} is not valid.");
                }
            }
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

        internal object ProcessObjectProp(string objectName, string propName)
        {
            var model = GetVarData(objectName);
            if (model != null) 
            {
                if (model is DynamicObj) 
                {                    
                    return ((DynamicObj)model).Get(propName);
                }

                var prop =  model.GetType().GetProperty(propName);
                if (prop != null)
                {
                    return prop.GetValue(model);
                }
                else {
                    throw new ScriptException($"Object at {objectName}.{propName} is not valid.");
                }
            }

            return null;
        }


        internal virtual object GetVarData(string propName)
        {
            if (propName.ToLower() == "output") return output;
            if (propName.ToLower() == "$context") return _appContext;

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
