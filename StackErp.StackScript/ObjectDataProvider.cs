using System;
using StackErp.Model;

namespace StackErp.StackScript
{
    internal class ObjectDataProvider
    {
        internal ObjectDataProvider()
        {

        }

        internal object GetObjectData(string objectName, string propName, Arguments args = null)
        {
            object val = null;
            switch(objectName) {
                case "_":
                    val = GetUtilityResult(propName, args);
                    break;
            }

            return val;
        }

        internal virtual object GetVarData(string propName)
        {
            return null;
        }

        private object GetUtilityResult(string propName, Arguments args)
        {
            if (UtilityFunctions.ContainsKey(propName))
                return UtilityFunctions.Get(propName).Invoke(args);
            
             throw new ScriptException("Invalid function _." + propName);
        }
    }

    internal class EntityModelDataProvider: ObjectDataProvider
    {
        EntityModelBase _modelBase;
        internal EntityModelDataProvider(EntityModelBase modelBase): base()
        {
            _modelBase = modelBase;
        }
        internal override object GetVarData(string propName)
        {
            return this._modelBase.GetValue(propName);
        }
    }
}
