using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StackErp.Model
{
    [JsonConverter(typeof(Utils.EntityCodeJsonConverter))]
    public struct EntityCode 
    {
        public int Code;
        public string Name;
        public static implicit operator EntityCode(int other)
        {
            EntityCode c;
            c.Code = other;
            c.Name = null;
            return c;
        }

        public override bool Equals(object obj)
        {
            return (int)obj == Code;
        }
        public override int GetHashCode()
        {
            if (Name == null)
            {
                return Code;
            }

            return Name.GetHashCode() ^ Code;
        }

        public static EntityCode Get(string name)
        {
            EntityCode c = AllEntities[name.ToUpper()];
            c.Name = name.ToUpper();
            return c;
        }
        public static Dictionary<string, int> AllEntities;

        public const int None = 0;
        public const int User = 1;
        public const int UserRole = 2;
    }
}