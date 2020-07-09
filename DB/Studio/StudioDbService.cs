using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.DB.Studio
{
    public class StudioDbService
    {        
        public void SaveEntity(DynamicObj entity)
        {
            // var dbModel = new DBModelBase(EntityCode.None);
            // foreach(var k in entity.Keys)
            // {
            //     //var fdata = new FieldData()
            //     //dbModel.Attributes.Add(k)
            // }
        }

        public void CreateTableColumn(string table, BaseField field)
        {
            
        }
    }
}