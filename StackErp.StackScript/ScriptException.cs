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
    public class ScriptException : Exception 
    {
        public ScriptException(string message): base(message)
        {
            
        }
    } 

    public class ScriptParsingException : ScriptException
    {
        public ScriptParsingException(string message)
            : base(message)
        { }
    }   

    public class ScriptRuntimeException : ScriptException
    {
        public ScriptRuntimeException(string message)
            : base(message)
        { }
    } 
}