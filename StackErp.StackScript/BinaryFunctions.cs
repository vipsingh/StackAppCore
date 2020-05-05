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
    public static class BinaryFunctions
    {
        static Dictionary<BinaryOperator, Function> _binder;
        static BinaryFunctions()
        {
            _binder = new Dictionary<BinaryOperator, Function>();
            _binder.Add(BinaryOperator.Plus, Add);
            _binder.Add(BinaryOperator.Equal, IsEqual);
        }

        public static Function Get(BinaryOperator op)
        {
            return _binder[op];
        }

        public static Function Add => new Function((arguments) =>
        {
            dynamic result = arguments.Get(0);
            foreach (dynamic ix in arguments.Skip(1)) {
                result += ix;
            }

            return result;
        });

        public static Function IsEqual => new Function((arguments) =>
        {
            var arg1 = arguments.Get(0);
            var arg2 = arguments.Get(1);
            if (arg1 == null && arg2 != null || arg1 != null && arg2 == null)
                return null;
            if (arg1 != null && !arg1.Equals(arg2))
                return (object)false;
                
            return (object)true;
        });
    }
}