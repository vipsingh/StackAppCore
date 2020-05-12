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
        public string ColorCode {set;get;}
        public string FontSize {set;get;}
        public string FontWeight {set;get;}
    }
}