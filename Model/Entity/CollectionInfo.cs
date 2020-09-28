using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StackErp.Model.Entity
{
    public class CollectionInfo
    {
        public int Id {get;}
        public string Name {get;}
        public CollectionType Type {get;}
        public DataSourceType SourceType {get;}
        public string SourceExp {get;}
        public string TextField {get;}
        public string ValueField {get;}
        public int MaxCount {set; get;}
        
        public CollectionInfo(int id, string name, int type, int sourceType, string sourceExp, string textField,string valueField)
        {
            Id = id;
            Name = name;
            Type = (CollectionType)type;
            SourceType = (DataSourceType)sourceType;
            SourceExp = sourceExp;
            TextField = textField;
            ValueField = valueField;
        }
    }

    public enum CollectionType
    {
        Global = 1,
        System = 2,
        Custom = 3
    }
}