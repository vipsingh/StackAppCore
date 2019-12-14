using System;
using System.Collections.Generic;

namespace Model.Entity
{
    public interface IDBEntity
    {
        string Name {set;get;}
        string DBName {set;get;}
        Dictionary<string, BaseField> Fields {get;}
        
        string FieldText {get; set;}

        List<string> Relations {set;get;}
        
    }
}