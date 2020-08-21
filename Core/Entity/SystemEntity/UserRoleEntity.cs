using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using StackErp.DB.Entity;
using StackErp.Model;
using StackErp.Model.Entity;

namespace StackErp.Core.Entity
{
    public class UserRoleEntity : DBEntity
    {
        public UserRoleEntity(int id, string name, Dictionary<string, BaseField> fields,EntityType entityType, string tableName) : base(id, name, fields, entityType, tableName)
        {
            this.Fields.Add("NAME", new StringField()
            {
                Type = FieldType.Text,
                Name = "name",
                Text = "Name",
                DBName = "name",
                IsRequired = true,
                Copy = false,
                IsDbStore = true,
                ViewId = 0
            });

            this.Fields.Add("INHERITBY", new IntegerField()
            {
                Type = FieldType.Text,
                Name = "InheritBy",
                Text = "InheritBy",
                DBName = "inheritby",
                Copy = false,
                IsDbStore = true,
                ViewId = 0
            });
        }

        public static IEnumerable<DbObject> GetUserRoles(int userId)
        {
            var roles = UserDbService.GetUserRoles(userId);

            return roles;
        }

        public static List<DynamicObj> GetEntityAccessData(int masterId, int roleId)
        {
            var data = UserDbService.GetEntityAccessData(masterId, roleId);

            return data;
        }
    }
}