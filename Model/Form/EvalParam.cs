using System;
using StackErp.Model;

namespace StackErp.Model.Form
{
    public class EvalParam
    {
        public string Name { set; get; }
        public EvalParamValue Value { set; get; }
    }

    public class EvalParamValue
    {
        public EvalSourceType Source { set; get; }
        public object Value { set; get; }
    }

}
