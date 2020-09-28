using System;
using System.Collections.Generic;
using StackErp.Model.DataList;

namespace StackErp.Model.Entity
{
    public interface IEntityDataFormatter
    {
        
    }

    public class FormatInfo
    {
        public TypeCode FieldBaseType {set;get;}
        public string FormatString {set;get;}
        public int DecimalPlace {set;get;}
        public string ColorCode {set;get;}
        public string FontSize {set;get;}
        public string FontWeight {set;get;}

        public string GetTypeString()
        {
            if ((new List<int>() {7,8,9,10,11,12}).Contains((int)FieldBaseType))
            {
                return "NUMBER";
            }
            else if ((new List<int>() {14,15}).Contains((int)FieldBaseType))
            {
                return "DECIMAL";
            }
            else if ((new List<int>() {16}).Contains((int)FieldBaseType))
            {
                return "DATETIME";
            }
            return null;
        }
    }
}