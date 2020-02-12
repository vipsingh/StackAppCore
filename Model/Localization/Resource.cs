using System;

namespace StackErp.Model.Localization
{
    public class Localize
    {
        public static string Get(string id, string defaultVal = null)
        {
            if(string.IsNullOrEmpty(defaultVal))
                return id;
            return defaultVal;
        }
    }
}
