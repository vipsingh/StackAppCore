using System;
using System.Collections.Generic;
using StackErp.Model;
using System.Linq;
using StackErp.Model.Entity;
using StackErp.Model.Form;

namespace StackErp.Core.Entity.Service
{
    public class DataComputationService
    {
        public static object ComputeFieldData(BaseField field, EntityModelBase modelBase)
        {
            var exp = field.ComputeExpression;
            if (exp == null) return null;
                        

            return null;
        }

        private static object Compute(EvalExpression exp, EntityModelBase modelBase)
        {
            return null;
        }
    }
}