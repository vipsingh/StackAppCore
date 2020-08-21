using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Esprima;
using Newtonsoft.Json;
using System.Globalization;
using StackErp.Model;

namespace StackErp.StackScript
{
    public delegate object Function(Arguments arguments);
    public static class ScriptFunctions
    {
        static InvariantDictionary<Function> _binder;
        static ScriptFunctions()
        {
            _binder = new InvariantDictionary<Function>();
            #region DataTypes
            _binder.Add("Collection", Collection);
            _binder.Add("Object", Obj);
            _binder.Add("Date", Date);
            #endregion

            _binder.Add("iftrue", IfTrue);
            _binder.Add("ifnull", IfNull);

            _binder.Add("contains", Contains);

            _binder.Add("log", Log);
            _binder.Add("throw", Throw);

            //_binder.Add("navigate", Navigate);
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

        internal static Function Log => new Function((arguments) =>
        {
            System.Diagnostics.Debug.WriteLine(arguments.Get(0));
            return null;
        });

        internal static Function Throw => new Function((arguments) =>
        {
            throw new ScriptUserException(arguments.Get(0).ToString());            
        });

        #region DataType
        internal static Function Collection => new Function((arguments) =>
        {
            if (arguments == null || arguments.Count == 0)
            {
                return new List<object>();
            }
            else 
            {
                var arg1 = arguments.Get(0);
                if (arg1 is string)
                {
                    var list = new List<string>();
                    list.AddRange(arguments.Cast<string>());
                    return list;
                }
                else if (arg1 is int)
                {
                    var list = new List<int>();
                    list.AddRange(arguments.Cast<int>());
                    return list;
                }
                else if (arg1 is decimal)
                {
                    var list = new List<decimal>();
                    list.AddRange(arguments.Cast<decimal>());
                    return list;
                }

                return new List<object>();
            }                        
        });
        internal static Function Obj => new Function((arguments) =>
        {
            var o = new DynamicObj();
            //
            return o;
        });
        internal static Function Date => new Function((arguments) =>
        {
            if (arguments == null || arguments.Count == 0) return DateTime.Now;
            else 
            {
                var arg1 = arguments.Get(0);
                if (!(arg1 is DateTime))
                    throw new ScriptException("Invalid argument in Date. Only iso date format string allowed.");

                return DateTime.Parse(arg1.ToString());
            }
        });
        #endregion

        #region Conditional
        internal static Function IfTrue => new Function((arguments) =>
        {
            if (arguments.Count <= 1 )
                throw new ScriptException("Invalid arguments in IfTrue");

            var arg1 = arguments.Get(0);
            if (!(arg1 is Boolean))
                throw new ScriptException("Invalid condition in IfTrue");

            if ((Boolean)arg1 == true) {
                return arguments.Get(1);
            } else {
                return arguments.Count > 2 ? arguments.Get(2) : null;
            }
        });

        internal static Function IfNull => new Function((arguments) =>
        {
            if (arguments.Count <= 1 )
                throw new ScriptException("Invalid arguments in IfNull");

            var arg1 = arguments.Get(0);

            if (arg1 == null) {
                return arguments.Get(1);
            } else {
                return arguments.Count > 2 ? arguments.Get(2) : null;
            }
        });
        #endregion
        
        #region Utility
        internal static Function Contains => new Function((arguments) =>
        {
            if (arguments.Count <= 1 )
                throw new ScriptException("Invalid arguments in Contains");

            var arg1 = arguments.Get(0);
            var arg2 = arguments.Get(1);

            if (arg1 is string) {
                if (arg2 is string)
                    return ((string)arg1).Contains((string)arg2);
                else
                    throw new ScriptException("Invalid second argument in Contains");
            }
            else if (arg1 is IList)
            {
                return ((IList)arg1).Contains(arg2);
            }

            throw new ScriptException("Invalid arguments in Contains.");
        });
        #endregion        
    } 

    public class Arguments : IEnumerable<object>
    {
        List<object> _list = new List<object>();

        /// <summary>
        /// Creates an empty arguments instance.
        /// </summary>
        public Arguments()
        { }

        /// <summary>
        /// Initializes the instance with the specified initial arguments.
        /// </summary>
        /// <param name="arguments">Arguments to initialize instance with.</param>
        public Arguments(params object[] arguments)
        {
            _list.AddRange(arguments);
        }

        /// <summary>
        /// Initializes the instance with the specified initial arguments.
        /// </summary>
        /// <param name="arguments">Arguments to initialize instance with.</param>
        public Arguments(IEnumerable<object> arguments)
        {
            _list.AddRange(arguments);
        }

        /// <summary>
        /// Returns the number of arguments in this instance.
        /// </summary>
        /// <value>The number of arguments this instance holds.</value>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Adds the specified argument to this instance.
        /// </summary>
        /// <param name="value">Argument to add.</param>
        public void Add(object value)
        {
            _list.Add(value);
        }

        /// <summary>
        /// Returns the argument at the specified instance.
        /// If you try to retrieve an argument at an index beyond the number of
        /// arguments that exists, the method will return "defaultValue".
        /// </summary>
        /// <returns>The argument at the specified index, or "defaultValue" if argument doesn't exist.</returns>
        /// <param name="index">The index of the argument you want to retrieve.</param>
        /// <param name="defaultValue">The default value to return if the argument doesn't exist.</param>
        public object Get(int index, object defaultValue = null)
        {
            if (index >= _list.Count)
                return defaultValue;
            return _list[index];
        }

        /// <summary>
        /// Returns the argument at the specified instance, and tries to convert
        /// it the the requested TConvert type. Will throw if you
        /// try to retrieve arguments beyond its size.
        /// </summary>
        /// <returns>The argument at the specified index.</returns>
        /// <param name="index">The index of the argument you want to retrieve.</param>
        /// <typeparam name="T">The type you want to convert the argument to.</typeparam>
        /// <param name="defaultValue">The default value to return if the argument doesn't exist.</param>
        public T Get<T>(int index, T defaultValue = default(T))
        {
            // Retrieving argument and converting it to type specified by caller.
            var obj = Get(index);
            if (obj == null)
                return defaultValue;
            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }

        public Type[] ToTypeArray()
        {
            var arr = new Type[_list.Count];
            for(int i=0;i<_list.Count;i++)
            {
                arr[i] = _list[i].GetType();
            }

            return arr;
        }

        #region [ -- Interface implementations -- ]

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<object> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}