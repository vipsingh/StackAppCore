using System;
using System.Data;
using System.Collections.Generic;
using StackErp.Model;

namespace StackErp.DB.Entity
{
    public class UserDbService
    {
        private static string GET_USER_ROLE = "select t_role.id, t_role.name from t_userrole join t_role on t_userrole.roleid = t_role.id where userid=@id";
        private static string GET_ENTITY_ACCESS_DATA = "select * from t_role_operation where masterId=@masterId and roleid=@id";
        private static string GET_INFO_FOR_AUTH = "select id, password, emailid from t_user where loginid=@lid";
        public static IEnumerable<DbObject> GetUserRoles(int userId)
        {
             var d = DBService.Query(GET_USER_ROLE, new {id = userId });

             return d;
        }

        public static IEnumerable<DbObject> GetUserInfoForAuth(string loginid)
        {
             var d = DBService.Query(GET_INFO_FOR_AUTH, new {lid = loginid });

             return d;
        }

        public static List<DynamicObj> GetEntityAccessData(int masterId, int roleId)
        {
            var data = new List<DynamicObj>();

            DBService.Query(GET_ENTITY_ACCESS_DATA, new {id = roleId, masterId }, (reader) => {               
                data.Add(reader.ReadAsDynamicObj());
            });

             return data;
        }
    }
}  