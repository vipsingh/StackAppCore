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
    public static class UtilityFunctions
    {
        static InvariantDictionary<Function> _binder;
        static UtilityFunctions()
        {
            _binder = new InvariantDictionary<Function>();

            _binder.Add("tovalue", ToValue);

            _binder.Add("parseint", ParseInt);
            _binder.Add("parsedecimal", ParseDecimal);
            _binder.Add("parsedatetime", ParseDateTime);
            _binder.Add("parsebool", ParseBool);

            _binder.Add("isint", IsInt);
            _binder.Add("isdecimal", IsDecimal);
            _binder.Add("isdateTime", IsDateTime);
            _binder.Add("isbool", IsBool);

            _binder.Add("concat", Concat);
        }
        internal static Function Get(string name)
        {
            return _binder[name];
        }

        internal static bool ContainsKey(string name)
        {
            return _binder.ContainsKey(name);
        }

        public static void RegisterFunction(string name, Function function)
        {
            if (!_binder.ContainsKey(name))
                _binder.Add(name, function);
        }

        //fetch value from any object that is returened from client or other source.
        internal static Function ToValue => new Function((arguments) =>
        {
            if (arguments.Count == 0 )
                throw new ScriptException("Invalid arguments in ToValue");
            var arg = arguments.Get(0);
            //argument can be jsonobject, dynamicobj, simple value
            var val = DataHelper.ResolveWidgetValue(arg);
            return val;
        });

        #region DataType related
        internal static Function ParseInt => new Function((arguments) =>
        {
            if (arguments.Count == 0 )
                throw new ScriptException("Invalid arguments in ParseInt");
            var val = arguments.Get(0);

            return DataHelper.GetDataValue(val, TypeCode.Int32);
        });

        internal static Function ParseDecimal => new Function((arguments) =>
        {
            if (arguments.Count == 0 )
                throw new ScriptException("Invalid arguments in ParseDecimal");
            var val = arguments.Get(0);

            return DataHelper.GetDataValue(val, TypeCode.Decimal);
        });

        internal static Function ParseDateTime => new Function((arguments) =>
        {
            if (arguments.Count == 0 )
                throw new ScriptException("Invalid arguments in ParseDateTime");
            var val = arguments.Get(0);

            return DataHelper.GetDataValue(val, TypeCode.DateTime);
        });

        internal static Function ParseBool => new Function((arguments) =>
        {
            if (arguments.Count == 0 )
                throw new ScriptException("Invalid arguments in ParseBool");
            var val = arguments.Get(0);

            return DataHelper.GetDataValue(val, TypeCode.Boolean);
        });
        
        internal static Function IsInt => new Function((arguments) =>
        {
            if (arguments.Count == 0 )
                throw new ScriptException("Invalid arguments in IsInt");
            var val = arguments.Get(0);

            return val is int;
        });
        internal static Function IsDecimal => new Function((arguments) =>
        {
            if (arguments.Count == 0 )
                throw new ScriptException("Invalid arguments in IsDecimal");
            var val = arguments.Get(0);

            return val is Decimal;
        });
        internal static Function IsDateTime => new Function((arguments) =>
        {
            if (arguments.Count == 0 )
                throw new ScriptException("Invalid arguments in IsDateTime");
            var val = arguments.Get(0);

            return val is DateTime;
        });
        internal static Function IsBool => new Function((arguments) =>
        {
            if (arguments.Count == 0 )
                throw new ScriptException("Invalid arguments in IsBool");
            var val = arguments.Get(0);

            return val is bool;
        });
        #endregion
    
        #region String
        internal static Function Concat => new Function((arguments) =>
        {
            if (arguments.Count == 0 )
                throw new ScriptException("Invalid arguments in Concat");

            return string.Concat(arguments.ToArray());
        });
        #endregion
    }
}