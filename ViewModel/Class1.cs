using System;
using System.Collections.Generic;
using StackErp.Core;
using StackErp.Model;
using StackErp.StackScript;
using StackErp.ViewModel.Model;

namespace StackErp.ViewModel
{
    public static class RegisterStackScriptRef
    {
        public static void Register()
        {
            #region Class Function Register
            ScriptObjectTypeFactory.Register(typeof(FieldRequestInfo), "GetFieldValue");
                        
            ScriptObjectTypeFactory.Register(typeof(FieldActionResponse), "AddFieldValue", "AddFieldProps");

            #endregion

            #region Functions
            ScriptFunctions.RegisterFunction("GetEntity", GetEntity);
            #endregion
        }

        #region Functions
        public static Function GetEntity => new Function((arguments) =>
        {
            var arg1 = arguments.Get(0);
            int entId = 0;
            if (arg1 is string)
            {
                entId = EntityCode.Get(arg1.ToString()).Code;
            }
            else
                entId = (int)arg1;
            
            return EntityMetaData.Get(entId);
        });

        #endregion
    }
}
