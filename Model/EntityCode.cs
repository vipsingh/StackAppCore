using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using StackErp.Model.Utils;

namespace StackErp.Model
{
    [JsonConverter(typeof(EntityCodeJsonConverter))]
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
            if (obj is EntityCode) return ((EntityCode)obj).Code == Code;
            
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
        public const int EntityMaster = 1;
        public const int EntitySchema = 2;
        public const int ItemType = 3;
        public const int EntityLayout = 4;
        public const int EntityList = 5;
        public const int EntityAction = 6;
        public const int Collection = 11;

        public const int User = 101;
        public const int UserRole = 102;  

        public const int Customer = 111;
        public const int Product = 112;  
    }

    public struct AppModuleCode 
    {
        public int Code;
        public string Name;
        public static implicit operator AppModuleCode(int other)
        {
            AppModuleCode c;
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

        public static AppModuleCode Get(string name)
        {
            AppModuleCode c = AllModules[name.ToUpper()];
            c.Name = name.ToUpper();
            return c;
        }
        public static Dictionary<string, int> AllModules;

        public const int None = 0;
        public const int Core = 1;
        public const int Sale = 10;
        public const int Purchase = 11;
        public const int Inventory = 12;
        public const int Finance = 13;
        public const int Hrm = 14;
    }
}