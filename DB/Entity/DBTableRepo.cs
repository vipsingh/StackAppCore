using System;
using System.Collections;
using System.Collections.Generic;
using Model.Entity;

namespace DB
{
    public class DBTableRepo<T> where T : Model.DBModel.DBBaseModel
    {
        public string Name {set;get;}
        public string DBName {set;get;}

        
    }

}