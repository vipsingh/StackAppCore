using System;
using StackErp.Model.Localization;
using StackErp.ViewModel.Model;

namespace StackErp.ViewModel.Helper
{
    public class ValidationHelper
    {
        public static IValidation GetRequiredValidation(string label)
        {
            return new RequiredValidation()
            {
                Msg = string.Format(Localize.Get("LBL_REQUIRED", "{0} is required"), label)
            };
        }
    }
}
