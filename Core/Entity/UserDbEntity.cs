using System;
using System.Collections.Generic;
using StackErp.Model.Entity;

namespace StackErp.Core.Entity
{
    public class UserDbEntity: DBEntity
    {
        public UserDbEntity(int id, string name, Dictionary<string, BaseField> fields, string tableName): base(id, name, fields, tableName)
        {
        }
    }
}